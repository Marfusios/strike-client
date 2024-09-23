using System.Diagnostics;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("Receives {Count}")]
public record ReceivesCollection : ResponseBase
{
	/// <summary>
	/// The page items.
	/// </summary>
	public IReadOnlyCollection<Receive> Items { get; init; } = [];

	/// <summary>
	/// Total number of records
	/// </summary>
	public int Count { get; init; }
}
