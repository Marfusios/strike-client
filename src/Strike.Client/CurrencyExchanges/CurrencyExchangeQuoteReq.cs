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
	/// <para>Currency code</para>
	/// </summary>
	public required CurrencyExchangeAmount Amount { get; set; }
}
