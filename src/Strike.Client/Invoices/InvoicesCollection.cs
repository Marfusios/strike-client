namespace Strike.Client.Invoices;
public record InvoicesCollection : ResponseBase
{
	/// <summary>
	/// The page items.
	/// </summary>
	public IReadOnlyCollection<Invoice> Items { get; init; } = [];

	/// <summary>
	/// Total number of records
	/// </summary>
	public int Count { get; init; }
}
