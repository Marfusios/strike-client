namespace Strike.Client;

/// <summary>
/// Provides common members for all Strike API responses
/// </summary>
public abstract record ResponseBase
{
	/// <summary>
	/// Holds the raw json returned by the server
	/// </summary>
	[JsonIgnore]
	public string? RawJson { get; internal set; }

	/// <summary>
	/// An error response given by the server
	/// </summary>
	public StrikeError? Error { get; init; }

	/// <summary>
	/// The http status code.
	/// </summary>
	public HttpStatusCode StatusCode { get; internal set; }

	/// <summary>
	/// A value indicating whether this instance is success status code.
	/// </summary>
	/// <value><c>true</c> if this instance is success status code.</value>
	public bool IsSuccessStatusCode => StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.Ambiguous;
}
