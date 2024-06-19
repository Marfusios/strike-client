using CommunityToolkit.Diagnostics;
using Strike.Client.Converters;

namespace Strike.Client;

/// <summary>
/// Provides methods for sending request to and receiving data from Strike API
/// </summary>
public sealed partial class StrikeClient
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StrikeClient"/> class using parameters that can all come from dependency injection.
	/// </summary>
	/// <param name="options"><see cref="StrikeOptions"/> initialized from an IConfiguration section</param>
	/// <param name="httpClientFactory">A factory instance used to create <see cref="HttpClient" /> instances. If one is not provided, a service collection will be created and used instead. For more information, see <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests"/> for more information.</param>
	/// <param name="logger">A logging instance. Log entries will be provided at Information level at completion of each api call; and at Trace level with request and content details at the start and end of each api call. If not provided, a <see cref="NullLogger" /> instance will be used.</param>
	public StrikeClient(
		IOptions<StrikeOptions> options,
		IHttpClientFactory? httpClientFactory = null,
		ILogger<StrikeClient>? logger = null)
		: this(
			  options.Value.Environment,
			  options.Value.AccessToken,
			  httpClientFactory: httpClientFactory,
			  logger: logger)
	{ }

	/// <summary>
	/// Initializes a new instance of the <see cref="StrikeClient"/> class.
	/// </summary>
	/// <param name="environment">The environment.</param>
	/// <param name="accessToken">The access token.</param>
	/// <param name="httpClientFactory">A factory instance used to create <see cref="HttpClient" /> instances. If one is not provided, a service collection will be created and used instead. For more information, see <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests"/> for more information.</param>
	/// <param name="logger">A logging instance. Log entries will be provided at Information level at completion of each api call; and at Trace level with request and content details at the start and end of each api call. If not provided, a <see cref="NullLogger" /> instance will be used.</param>
	/// <remarks>
	/// Usage patterns: 
	/// A single <see cref="StrikeClient"/> may be used for all API calls, or a separate one may be used for each
	/// If the <paramref name="accessToken"/> is provided, it will be used on every call
	/// </remarks>
	public StrikeClient(
		Environment environment,
		string? accessToken = null,
		IHttpClientFactory? httpClientFactory = null,
		ILogger<StrikeClient>? logger = null)
	{
		_baseUrl = environment switch
		{
			Environment.Development => new Uri("https://api.dev.strike.me/"),
			Environment.Live => new Uri("https://api.strike.me/"),
			_ => throw new ArgumentOutOfRangeException(nameof(environment), "Invalid environment provided."),
		};

		AccessToken = string.IsNullOrWhiteSpace(accessToken) ? null : accessToken;

		if (httpClientFactory == null)
		{
			var collection = new ServiceCollection();
			_ = collection.AddStrikeHttpClient();
			var serviceProvider = collection.BuildServiceProvider();
			_clientFactory = serviceProvider.GetService<IHttpClientFactory>()!;
		}
		else
			_clientFactory = httpClientFactory;

		_logger = logger ?? new NullLogger<StrikeClient>();
	}

	private readonly Uri _baseUrl;
	private readonly IHttpClientFactory _clientFactory;
	private readonly ILogger _logger;

	internal static readonly JsonSerializerOptions JsonSerializerOptions =
		new JsonSerializerOptions()
		{
#if DEBUG
			WriteIndented = true,
#else
			WriteIndented = false,
#endif
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNameCaseInsensitive = true,
		}
		.AddStrikeConverters();

	/// <summary>
	/// Name of the HTTP client used for dependency injection resolution.
	/// </summary>
	public static readonly string HttpClientName = "StrikeClient";

	/// <summary>
	/// The access token used for all API calls.
	/// </summary>
	public string? AccessToken { get; set; }

	/// <summary>
	/// Debug option to include the raw json in the returned DTO
	/// </summary>
	public bool ShowRawJson { get; set; }

	/// <summary>
	/// Additional request headers used for all API calls.
	/// </summary>
	public Dictionary<string, string>? AdditionalHeaders { get; } = [];

	private ResponseParser PostAsync<TRequest>(string path, TRequest request) where TRequest : RequestBase
	{
		Guard.IsNotNull(request);

		var client = _clientFactory.CreateClient(HttpClientName);
		var url = new Uri(_baseUrl, path);
		_logger.LogTrace("Initiating request. Method: {Method}; Url: {Url}; Content: {@Content}", "POST", url, request);

#pragma warning disable CA2000 // Dispose objects before losing scope
		var requestMessage = new HttpRequestMessage
		{
			Method = HttpMethod.Post,
			RequestUri = url,
			Content = JsonContent.Create(request, options: JsonSerializerOptions),
		};
#pragma warning restore CA2000 // Dispose objects before losing scope

		AddAuthenticationHeader(requestMessage);
		AddRequestHeaders(requestMessage, AdditionalHeaders);
		AddRequestHeaders(requestMessage, request.AdditionalHeaders);

		return new ResponseParser
		{
			Message = client.SendAsync(requestMessage),
			Url = url.ToString(),
			IncludeRawJson = request.ShowRawJson ?? ShowRawJson,
			Logger = _logger,
		};
	}

	private ResponseParser GetAsync(string path)
	{
		var client = _clientFactory.CreateClient(HttpClientName);
		var url = new Uri(_baseUrl, path);
		_logger.LogTrace("Initiating request. Method: {Method}; Url: {Url}", "GET", url);

#pragma warning disable CA2000 // Dispose objects before losing scope
		var requestMessage = new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = url
		};
#pragma warning restore CA2000 // Dispose objects before losing scope

		AddAuthenticationHeader(requestMessage);
		AddRequestHeaders(requestMessage, AdditionalHeaders);

		return new ResponseParser
		{
			Message = client.SendAsync(requestMessage),
			Url = url.ToString(),
			IncludeRawJson = ShowRawJson,
			Logger = _logger,
		};
	}

	private void AddAuthenticationHeader(HttpRequestMessage requestMessage)
	{
		requestMessage.Headers.Add("Authorization", $"Bearer {AccessToken}");
	}

	private static void AddRequestHeaders(HttpRequestMessage requestMessage, Dictionary<string, string>? headers)
	{
		if (headers != null)
		{
			foreach (var header in headers)
			{
				requestMessage.Headers.Add(header.Key, header.Value);
			}
		}
	}

	private readonly struct ResponseParser
	{
		public Task<HttpResponseMessage> Message { get; init; }
		public string Url { get; init; }
		public bool IncludeRawJson { get; init; }
		public ILogger Logger { get; init; }

		public async Task<TResponse> ParseResponseAsync<TResponse>() where TResponse : ResponseBase, new()
		{
			using var response = await Message.ConfigureAwait(false);

			Logger.LogInformation("Completed request. Url: {Url}, Status Code: {StatusCode}.", Url, response.StatusCode);

			var result = await BuildResponse<TResponse>(response).ConfigureAwait(false);
			Logger.LogTrace("Completed request details. Url: {Url}; Response: {@Result}",
				Url,
				result);
			return result;
		}

		private async Task<TResponse> BuildResponse<TResponse>(HttpResponseMessage response) where TResponse : ResponseBase, new()
		{
			if (response.IsSuccessStatusCode)
			{
				if (IncludeRawJson)
				{
					var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					var result = JsonSerializer.Deserialize<TResponse>(json, options: JsonSerializerOptions);
					result!.RawJson = json;
					result.StatusCode = response.StatusCode;
					return result;
				}
				else
				{
					var result = await response.Content.ReadFromJsonAsync<TResponse>(options: JsonSerializerOptions).ConfigureAwait(false);
					result!.StatusCode = response.StatusCode;
					return result;
				}
			}
			else
			{
				var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

				var error = ParseError((int)response.StatusCode, json);
				var result = new TResponse
				{
					Error = error,
					StatusCode = response.StatusCode,
				};

				if (IncludeRawJson)
					result.RawJson = json;
				return result;
			}
		}

		private static StrikeError ParseError(int statusCode, string json)
		{
			try
			{
				return JsonSerializer.Deserialize<StrikeError>(json, options: JsonSerializerOptions)!;
			}
			catch (JsonException ex)
			{
				return new StrikeError
				{
					Data = new StrikeApiError
					{
						Status = statusCode,
						Code = "API_UNAVAILABLE",
						Message = ex.Message
					}
				};
			}
		}
	}
}
