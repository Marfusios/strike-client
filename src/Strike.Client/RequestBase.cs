namespace Strike.Client;

/// <summary>
/// Provides methods and properties for making a standard request.
/// </summary>
public abstract class RequestBase
{
	/// <summary>
	/// Enable to receive the raw JSON provided by the Strike server.
	/// Values:
	/// <list type="table">
	/// <item><term><see langword="false" /></term><description>Do not return the raw JSON.</description></item>
	/// <item><term><see langword="true" /></term><description>Return the raw JSON.</description></item>
	/// <item><term><see langword="null" /></term><description>Use the value in <see cref="StrikeClient.ShowRawJson"/>.</description></item>
	/// </list>
	/// </summary>
	[JsonIgnore]
	public bool? ShowRawJson { get; set; }

	/// <summary>
	/// Pass additional request headers.
	/// </summary>
	[JsonIgnore]
	public Dictionary<string, string>? AdditionalHeaders { get; init; }
}
