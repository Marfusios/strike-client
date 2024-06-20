namespace Strike.Client.Invoices;

/// <summary>
/// All supported currencies
/// </summary>
public enum InvoiceState
{
	/// <summary>
	/// Invoice is ready to be paid
	/// </summary>
	[EnumMember(Value = "UNPAID")]
	Unpaid,

	/// <summary>
	/// Invoice is in process of being paid
	/// </summary>
	[EnumMember(Value = "PENDING")]
	Pending,

	/// <summary>
	/// Invoice is paid
	/// </summary>
	[EnumMember(Value = "PAID")]
	Paid,

	/// <summary>
	/// Invoice is explicitly cancelled
	/// </summary>
	[EnumMember(Value = "CANCELLED")]
	Canceled,

	/// <summary>
	/// Invoice is reversed
	/// </summary>
	[EnumMember(Value = "REVERSED")]
	Reversed,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
