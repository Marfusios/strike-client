namespace Strike.Client.PaymentQuotes.Onchain;
public record OnchainPaymentQuote : PaymentQuote
{
	/// <summary>
	/// Estimated delivery duration in minutes. For onchain payments, this is the time it takes for the transaction to be mined.
	/// </summary>
	/// <example>10</example>
	public uint? EstimatedDeliveryDurationInMin { get; init; }
}
