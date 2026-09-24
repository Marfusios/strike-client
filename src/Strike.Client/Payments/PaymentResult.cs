namespace Strike.Client.Payments;

/// <summary>
/// Legacy payment execution result. Use PaymentState for current integrations.
/// </summary>
public enum PaymentResult
{
	[EnumMember(Value = "PENDING")]
	Pending,

	[EnumMember(Value = "SUCCESS")]
	Success,

	[EnumMember(Value = "FAILURE")]
	Failure,

	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
