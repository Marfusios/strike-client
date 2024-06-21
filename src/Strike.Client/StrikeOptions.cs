namespace Strike.Client;

/// <summary>
/// Implements the IOptions pattern for reading a 'Strike' section from an IConfiguration object.
/// </summary>
public class StrikeOptions
{
	/// <summary>
	/// A default name in the IConfiguration that contains StrikeOptions settings
	/// </summary>
	/// <remarks>
	/// Section name in configuration is configurable. This default is offered for
	/// convenience only.
	/// </remarks>
	public const string SectionKey = "Strike";

	/// <summary>
	/// The default key used to authenticate API requests
	/// </summary>
	public string? ApiKey { get; set; }

	/// <summary>
	/// Target Strike environment
	/// <see cref="Client.Environment"/>: Development | Live
	/// </summary>
	public Environment Environment { get; set; } = Environment.Live;

	/// <summary>
	/// Name of the HTTP client used for dependency injection resolution.
	/// </summary>
	public const string HttpClientName = "StrikeClient";

	/// <summary>
	/// Get correct Strike url based on environment
	/// </summary>
	public static Uri ResolveServerUrl(Environment environment)
	{
		return environment switch
		{
			Environment.Development => new Uri("https://api.dev.strike.me/"),
			Environment.Live => new Uri("https://api.strike.me/"),
			_ => throw new ArgumentOutOfRangeException(nameof(environment), "Invalid environment provided."),
		};
	}
}
