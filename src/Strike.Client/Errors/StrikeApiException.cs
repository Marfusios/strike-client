namespace Strike.Client.Errors;

public class StrikeApiException : Exception
{
	public StrikeApiException()
	{
	}

	public StrikeApiException(string? message) : base(message)
	{
	}

	public StrikeApiException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	public StrikeApiError? Error { get; init; }
}
