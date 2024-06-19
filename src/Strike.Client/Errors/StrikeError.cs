namespace Strike.Client.Errors;

/// <summary>
/// The error that is returned when a response from the Strike API contains an error.
/// </summary>
public record StrikeError
{
	/// <summary>
	/// Unique identifier which represents this request in trace logs.
	/// </summary>
	public string? TraceId { get; init; } = null!;

	/// <summary>
	/// Error details
	/// </summary>
	public StrikeApiError Data { get; init; } = null!;
}
