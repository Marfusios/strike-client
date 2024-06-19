using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes;

/// <summary>
/// 
/// </summary>
public partial class CreateLnurlPaymentQuoteReq : RequestBase
{
	/// <summary>
	/// <para>LN Address or LNURL to pay to.</para>
	/// </summary>
	public required string LnAddressOrUrl { get; set; }

	/// <summary>
	/// <para>Currency to spend. If BTC is specified then <c>amount.currency</c> must also be BTC.</para>
	/// </summary>
	public required Currency SourceCurrency { get; set; }

	/// <summary>
	/// <para>Amount to send.</para>
	/// </summary>
	public required Money Amount { get; set; }

	/// <summary>
	/// <para>Payment description. Can be set only if the LNURL service allows comment.</para>
	/// <para>Max length of the description is defined by the commentAllowed property of the GET LNURL details endpoint response.</para>
	/// </summary>
	public string? Description { get; set; } = default!;

}
