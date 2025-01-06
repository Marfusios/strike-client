using System.Diagnostics;

namespace Strike.Client.Deposits;

[DebuggerDisplay("Deposits {Count}")]
public record DepositsCollection : ResponseBase
{
	/// <summary>
	/// The page items.
	/// </summary>
	public IReadOnlyCollection<Deposit> Items { get; init; } = [];

	/// <summary>
	/// Total number of records
	/// </summary>
	public int Count { get; init; }
}
