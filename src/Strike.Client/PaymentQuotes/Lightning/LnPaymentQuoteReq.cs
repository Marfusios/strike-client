using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes.Lightning;

/// <summary>
/// LN request
/// </summary>
public partial class LnPaymentQuoteReq : IdempotentRequestBase
{
	/// <summary>
	/// LN invoice to pay
	/// </summary>
	public required string LnInvoice { get; set; }

	/// <summary>
	/// Currency to send from. Defaults to the user's default currency.
	/// If BTC is specified then `amount.currency` must also be BTC.
	/// </summary>
	public Currency? SourceCurrency { get; set; }

	/// <summary>
	/// Amount to send if using zero amount invoice. Must be omitted otherwise. FeePolicy defaults to EXCLUSIVE.
	/// </summary>
	public MoneyWithFee? Amount { get; set; }

}
