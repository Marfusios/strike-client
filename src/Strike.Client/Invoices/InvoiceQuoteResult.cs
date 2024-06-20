namespace Strike.Client.Invoices;

/// <summary>
/// Quote result state
/// </summary>
public enum InvoiceQuoteResult
{
	/// <summary>
	/// The quote is not yet in a final state (it will be in this state even if the payment is started)
	/// </summary>
	[EnumMember(Value = "PENDING")]
	Pending,

	/// <summary>
	/// The quote has been paid
	/// </summary>
	[EnumMember(Value = "PAID")]
	Paid,

	/// <summary>
	/// The quote has expired
	/// </summary>
	[EnumMember(Value = "EXPIRED")]
	Expired,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
