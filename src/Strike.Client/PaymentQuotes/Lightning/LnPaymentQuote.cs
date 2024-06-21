using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes.Lightning;

/// <summary>
/// Lightning Network payment quote
/// </summary>
public record LnPaymentQuote : PaymentQuote
{
	/// <summary>
	/// <para>The fee required by LN network</para>
	/// </summary>
	public Money? LightningNetworkFee { get; init; }
}
