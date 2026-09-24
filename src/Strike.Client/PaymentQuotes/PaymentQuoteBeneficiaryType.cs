namespace Strike.Client.PaymentQuotes;

public enum PaymentQuoteBeneficiaryType
{
	[EnumMember(Value = "SELF")]
	Self,

	[EnumMember(Value = "INDIVIDUAL")]
	Individual,

	[EnumMember(Value = "BUSINESS")]
	Business,

	[EnumMember(Value = "UNKNOWN")]
	Unknown,

	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
