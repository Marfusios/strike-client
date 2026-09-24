namespace Strike.Client.PaymentQuotes;

public enum PaymentDestinationType
{
	[EnumMember(Value = "EXCHANGE_OR_PLATFORM")]
	ExchangeOrPlatform,

	[EnumMember(Value = "SELF_CUSTODY_WALLET")]
	SelfCustodyWallet,

	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
