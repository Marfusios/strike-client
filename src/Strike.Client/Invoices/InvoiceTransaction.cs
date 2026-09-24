using Strike.Client.Models;

namespace Strike.Client.Invoices;

/// <summary>
/// A transaction received against an invoice.
/// </summary>
public record InvoiceTransaction
{
	public Guid TransactionId { get; init; }
	public InvoiceTransactionState State { get; init; }
	public Money AmountReceived { get; init; } = default!;
	public DateTimeOffset Created { get; init; }
	public DateTimeOffset? Completed { get; init; }
}
