namespace Strike.Client.ReceiveRequests;
public record ReceiveRequestsCollection : ResponseBase
{
	/// <summary>
	/// The page items.
	/// </summary>
	public IReadOnlyCollection<ReceiveRequest> Items { get; init; } = [];

	/// <summary>
	/// Total number of records
	/// </summary>
	public int Count { get; init; }
}
