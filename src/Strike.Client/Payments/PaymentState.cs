namespace Strike.Client.Payments;

/// <summary>
/// Payment state
/// </summary>
public enum PaymentState
{
	/// <summary>
	/// The payment is not completed yet, but user was charged. Check state later
	/// </summary>
	[EnumMember(Value = "PENDING")]
	Pending,

	/// <summary>
	/// The payment was successful
	/// </summary>
	[EnumMember(Value = "COMPLETED")]
	Completed,

	/// <summary>
	/// The payment failed and user was not charged
	/// </summary>
	[EnumMember(Value = "FAILED")]
	Failed,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
