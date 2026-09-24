namespace Strike.Client.Errors;

/// <summary>Diagnostics returned by Strike in the development environment.</summary>
public record StrikeErrorDebug
{
	/// <summary>Full diagnostic information, including the stack trace.</summary>
	public string Full { get; init; } = string.Empty;

	/// <summary>Additional diagnostic information.</summary>
	public string? Body { get; init; }
}
