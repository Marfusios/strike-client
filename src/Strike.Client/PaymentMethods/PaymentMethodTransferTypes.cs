namespace Strike.Client.PaymentMethods;

/// <summary>
/// Payment state
/// </summary>
public enum PaymentMethodTransferTypes
{
	/// <summary>
	/// ACH
	/// </summary>
	[EnumMember(Value = "ACH")]
	ACH,
	
	/// <summary>
	/// US_DOMESTIC_WIRE
	/// </summary>
	[EnumMember(Value = "US_DOMESTIC_WIRE")]
	US_DOMESTIC_WIRE,
	
	/// <summary>
	/// SEPA
	/// </summary>
	[EnumMember(Value = "SEPA")]
	SEPA,
	
	/// <summary>
	/// FPS
	/// </summary>
	[EnumMember(Value = "FPS")]
	FPS,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
