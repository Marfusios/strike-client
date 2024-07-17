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

/// <summary>
/// Provides methods and properties for making an idempotent request (protection against double execution).
/// </summary>
public abstract class IdempotentRequestBase : RequestBase
{
	/// <summary>
	/// Set optional unique idempotency key to prevent double execution.
	/// In case of a duplicate request, the error code `DUPLICATE_XXX` containing data from the original response will be returned.
	/// The key should be a random v4 UUID to avoid false collisions. 
	/// </summary>
	public Guid? IdempotencyKey { get; set; }
}
