namespace Strike.Client.Errors;

/// <summary>
/// Strike API error
/// </summary>
public record StrikeApiError : StrikeErrorDetails
{
	/// <summary>
	/// Status code (specific for host - HTTP, etc.)
	/// </summary>
	public int Status { get; init; }

	/// <summary>
	/// Map of validation errors. Key is string and contains name of the field for which validation has failed.
	/// The value is an array of errors for that field.
	/// </summary>
	public IReadOnlyDictionary<string, StrikeErrorDetails[]>? ValidationErrors { get; init; }
}
