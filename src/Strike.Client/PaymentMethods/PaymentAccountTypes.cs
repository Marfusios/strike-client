namespace Strike.Client.PaymentMethods;

/// <summary>
/// Payment method account types
/// </summary>
public enum PaymentMethodAccountTypes
{
	/// <summary>
	/// CHECKING
	/// </summary>
	[EnumMember(Value = "CHECKING")]
	Checking,

	/// <summary>
	/// SAVINGS
	/// </summary>
	[EnumMember(Value = "SAVINGS")]
	Savings,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
