namespace Strike.Client.Models;

/// <summary>
/// <para>Currency to spend. If BTC is specified then <c>amount.currency</c> must also be BTC.</para>
/// </summary>
public enum Currency
{
	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "BTC")]
	Btc,

	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "USD")]
	Usd,

	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "EUR")]
	Eur,

	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "USDT")]
	Usdt,

	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "GBP")]
	Gbp,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "undefined")]
	Undefined
}
