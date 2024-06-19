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
	/// The default access token used to authenticate API requests
	/// </summary>
	public string? AccessToken { get; set; }

	/// <summary>
	/// Target Strike environment
	/// <see cref="Client.Environment"/>: Development | Live
	/// </summary>
	public Environment Environment { get; set; } = Environment.Live;
}
