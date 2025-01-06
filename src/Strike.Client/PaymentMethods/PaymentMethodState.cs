namespace Strike.Client.PaymentMethods;

/// <summary>
/// Payment method state
/// </summary>
public enum PaymentMethodState
{
	/// <summary>
	/// The payment method is pending
	/// </summary>
	[EnumMember(Value = "PENDING")]
	Pending,

	/// <summary>
	/// The payment method is ready to be used
	/// </summary>
	[EnumMember(Value = "READY")]
	Ready,

	/// <summary>
	/// The payment method is suspended
	/// </summary>
	[EnumMember(Value = "SUSPENDED")]
	Suspended,

	/// <summary>
	/// The payment method is invalid
	/// </summary>
	[EnumMember(Value = "INVALID")]
	Invalid,

	/// <summary>
	/// The payment method is inactive
	/// </summary>
	[EnumMember(Value = "INACTIVE")]
	Inactive,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
