using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Strike.Client.Deposits;
using Strike.Client.Errors;
using Strike.Client.Invoices;
using Strike.Client.Models;

namespace Strike.Client.Tests;

public class StrikeClientTests
{
	[Fact]
	public async Task GetBalances_ParsesResponseAndSendsHeaders()
	{
		const string Json = """[{"currency":"BTC","available":"0.12345678","outgoing":0.01,"total":"0.13345678"}]""";
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal(new Uri("https://strike.example/v1/balances"), request.RequestUri);
			Assert.Equal("Bearer test-key", request.Headers.Authorization?.ToString());
			Assert.Contains("StrikeClient/", request.Headers.UserAgent.ToString(), StringComparison.Ordinal);
			Assert.Equal("test-value", Assert.Single(request.Headers.GetValues("X-Test")));
			return JsonResponse(Json);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		client.AdditionalHeaders!.Add("X-Test", "test-value");
		client.ShowRawJson = true;

		var response = await client.Balances.GetBalances();

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(Json, response.RawJson);
		var balance = Assert.Single(response);
		Assert.Equal(Currency.Btc, balance.Currency);
		Assert.Equal(0.12345678m, balance.Available);
		Assert.Equal(0.01m, balance.Outgoing);
	}

	[Theory]
	[InlineData(null, true, true)]
	[InlineData(false, true, false)]
	[InlineData(true, false, true)]
	public async Task IssueInvoice_SerializesMoneyAndHonorsRawJsonOverride(bool? requestRawJson, bool clientRawJson, bool expectedRawJson)
	{
		using var handler = new StubHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/v1/invoices", request.RequestUri?.AbsolutePath);
			Assert.Equal("application/json", request.Content?.Headers.ContentType?.MediaType);
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			var amount = body.RootElement.GetProperty("Amount");
			Assert.Equal("12.34", amount.GetProperty("Amount").GetString());
			Assert.Equal("USD", amount.GetProperty("Currency").GetString());
			Assert.False(body.RootElement.TryGetProperty("ShowRawJson", out _));
			Assert.False(body.RootElement.TryGetProperty("AdditionalHeaders", out _));
			Assert.False(body.RootElement.TryGetProperty("Description", out _));
			return JsonResponse("{}");
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		client.ShowRawJson = clientRawJson;

		var response = await client.Invoices.IssueInvoice(new InvoiceReq
		{
			Amount = new Money { Amount = 12.34m, Currency = Currency.Usd },
			ShowRawJson = requestRawJson
		});

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(expectedRawJson ? "{}" : null, response.RawJson);
	}

	[Fact]
	public async Task CreateDeposit_SendsIdempotencyAndRequestHeaders()
	{
		var idempotencyKey = Guid.NewGuid();
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/v1/deposits", request.RequestUri?.AbsolutePath);
			Assert.Equal(idempotencyKey.ToString(), Assert.Single(request.Headers.GetValues("idempotency-key")));
			Assert.Equal("request-value", Assert.Single(request.Headers.GetValues("X-Request")));
			return JsonResponse("{}");
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.Deposits.Create(new DepositReq
		{
			PaymentMethodId = Guid.NewGuid(),
			Amount = "10.00",
			IdempotencyKey = idempotencyKey,
			AdditionalHeaders = new Dictionary<string, string> { ["X-Request"] = "request-value" }
		});

		Assert.True(response.IsSuccessStatusCode);
	}

	[Fact]
	public async Task GetInvoices_SendsPaginationAndParsesCollection()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal("?$top=20&$skip=40", request.RequestUri?.Query);
			return JsonResponse("""{"items":[{"invoiceId":"224bff37-021f-43e5-9b9c-390e3d834720","state":"PAID","created":"2026-09-24T10:00:00Z"}],"count":1}""");
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Invoices.GetInvoices(20, 40);

		Assert.True(response.IsSuccessStatusCode);
		var invoice = Assert.Single(response.Items);
		Assert.Equal(InvoiceState.Paid, invoice.State);
		Assert.Equal(new DateTimeOffset(2026, 9, 24, 10, 0, 0, TimeSpan.Zero), invoice.Created);
	}

	[Fact]
	public async Task ExecuteQuote_HandlesEmptySuccessResponse()
	{
		var quoteId = Guid.NewGuid();
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Patch, request.Method);
			Assert.Equal($"/v1/currency-exchange-quotes/{quoteId}/execute", request.RequestUri?.AbsolutePath);
			return JsonResponse(string.Empty, HttpStatusCode.NoContent);
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).CurrencyExchanges.ExecuteQuote(quoteId);

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
	}

	[Fact]
	public async Task ApiError_PreservesStatusAndValidationDetails()
	{
		const string Json = """{"traceId":"trace-1","data":{"status":400,"code":"INVALID_AMOUNT","message":"Invalid amount","validationErrors":{"amount":[{"code":"POSITIVE","message":"Must be positive"}]}}}""";
		using var handler = new StubHandler(_ => JsonResponse(Json, HttpStatusCode.BadRequest));
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		client.ShowRawJson = true;

		var response = await client.Balances.GetBalances();

		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Equal("trace-1", response.Error?.TraceId);
		Assert.Equal("INVALID_AMOUNT", response.Error?.Data.Code);
		Assert.Equal("POSITIVE", Assert.Single(response.Error!.Data.ValidationErrors!["amount"]).Code);
		Assert.Equal(Json, response.RawJson);
	}

	[Fact]
	public async Task ApiError_ThrowsWhenRequested()
	{
		using var handler = new StubHandler(_ => JsonResponse("""{"data":{"status":401,"code":"UNAUTHORIZED"}}""", HttpStatusCode.Unauthorized));
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		client.ThrowOnError = true;

		var error = await Assert.ThrowsAsync<StrikeApiException>(client.Balances.GetBalances);

		Assert.Contains("UNAUTHORIZED", error.Message, StringComparison.Ordinal);
	}

	[Theory]
	[InlineData("<html>Unavailable</html>", HttpStatusCode.BadGateway)]
	[InlineData("", HttpStatusCode.TooManyRequests)]
	public async Task NonJsonError_PreservesHttpStatus(string body, HttpStatusCode status)
	{
		using var handler = new StubHandler(_ => JsonResponse(body, status));
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Balances.GetBalances();

		Assert.Equal(status, response.StatusCode);
		Assert.Equal("API_UNAVAILABLE", response.Error?.Data.Code);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public async Task TransportFailure_HonorsThrowOnError(bool throwOnError)
	{
		using var handler = new StubHandler(_ => Task.FromException<HttpResponseMessage>(new HttpRequestException("Connection refused")));
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		client.ThrowOnError = throwOnError;

		if (throwOnError)
		{
			_ = await Assert.ThrowsAsync<HttpRequestException>(client.Balances.GetBalances);
			return;
		}

		var response = await client.Balances.GetBalances();

		Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
		Assert.Equal("API_UNAVAILABLE", response.Error?.Data.Code);
	}

	[Fact]
	public async Task AddStrike_BindsConfigurationAndUsesNamedHttpClient()
	{
		var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
		{
			["Strike:ApiKey"] = "configured-key",
			["Strike:Environment"] = "Development",
			["Strike:ServerUrl"] = "https://configured.example/"
		}).Build();
		var services = new ServiceCollection().AddStrike(configuration);
		_ = services.AddHttpClient(StrikeOptions.HttpClientName).ConfigurePrimaryHttpMessageHandler(() => new StubHandler(request =>
		{
			Assert.Equal("configured.example", request.RequestUri?.Host);
			Assert.Equal("Bearer configured-key", request.Headers.Authorization?.ToString());
			return JsonResponse("[]");
		}));
		using var provider = services.BuildServiceProvider();
		var client = provider.GetRequiredService<StrikeClient>();

		var response = await client.Balances.GetBalances();

		Assert.Equal(StrikeEnvironment.Custom, client.Environment);
		Assert.True(response.IsSuccessStatusCode);
		Assert.Empty(response);
	}

	private static StrikeClient CreateClient(HttpClient httpClient) =>
		new(StrikeEnvironment.Live, "test-key", new StubClientFactory(httpClient), serverUrl: new Uri("https://strike.example/"));

	private static HttpResponseMessage JsonResponse(string json, HttpStatusCode status = HttpStatusCode.OK) =>
		new(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

	private sealed class StubClientFactory(HttpClient client) : IHttpClientFactory
	{
		public HttpClient CreateClient(string name)
		{
			Assert.Equal(StrikeOptions.HttpClientName, name);
			return client;
		}
	}

	private sealed class StubHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> response) : HttpMessageHandler
	{
		public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> response)
			: this(request => Task.FromResult(response(request)))
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => response(request);
	}
}
