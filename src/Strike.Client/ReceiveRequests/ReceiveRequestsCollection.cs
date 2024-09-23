using System.Diagnostics;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("ReceiveRequests {Count}")]
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
