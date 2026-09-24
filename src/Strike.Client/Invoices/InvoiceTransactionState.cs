namespace Strike.Client.Invoices;

/// <summary>
/// State of a transaction received against an invoice.
/// </summary>
public enum InvoiceTransactionState
{
	[EnumMember(Value = "PENDING")]
	Pending,

	[EnumMember(Value = "COMPLETED")]
	Completed,

	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
