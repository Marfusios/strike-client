namespace Strike.Client.Errors;

/// <summary>
/// Strike error details
/// </summary>
public record StrikeErrorDetails
{
	/// <summary>
	/// Unique error code
	/// </summary>
	public string Code { get; init; } = default!;

	/// <summary>
	/// Default interpolated english error message
	/// </summary>
	public string? Message { get; init; }

	/// <summary>
	/// Optional parameters for the message
	/// </summary>
	public IReadOnlyDictionary<string, object?>? Values { get; init; }
}
