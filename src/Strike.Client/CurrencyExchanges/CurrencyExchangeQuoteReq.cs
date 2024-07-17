using Strike.Client.Models;

namespace Strike.Client.CurrencyExchanges;

public class CurrencyExchangeQuoteReq : RequestBase
{
	/// <summary>
	/// <para>Currency code to Sell</para>
	/// </summary>
	public required Currency Sell { get; set; }

	/// <summary>
	/// <para>Currency code to Buy</para>
	/// </summary>
	public required Currency Buy { get; set; }

	/// <summary>
	/// <para>Amount and currency to convert. The currency must match either `buy` or `sell`. FeePolicy defaults to EXCLUSIVE.</para>
	/// </summary>
	public required MoneyWithFee Amount { get; set; }
}
