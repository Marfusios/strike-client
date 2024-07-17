using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes.Onchain;
public class OnchainPaymentQuoteReq : IdempotentRequestBase
{
	/// <summary>
	/// BTC address to send funds to.
	/// </summary>
	/// <example>bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh</example>
	public required string BtcAddress { get; init; }

	/// <summary>
	/// Currency to spend. If BTC is specified then `amount.currency` must also be BTC.
	/// </summary>
	/// <example>USD</example>
	public required Currency SourceCurrency { get; init; }

	/// <summary>
	/// Payment description.
	/// </summary>
	/// <example>Payment for order #2383.</example>
	public string? Description { get; init; }

	/// <summary>
	/// Amount to send. FeePolicy defaults to EXCLUSIVE.
	/// </summary>
	public required MoneyWithFee Amount { get; init; }

	/// <summary>
	/// Id of the tier retrieved from `/payment-quotes/onchain/tiers`.
	/// </summary>
	/// <example>tier_fast</example>
	public required string OnchainTierId { get; init; }
}
