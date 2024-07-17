using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.CurrencyExchanges;

/// <summary>
/// Conversion details
/// </summary>
[DebuggerDisplay("{Amount} {Currency}, {FeePolicy}")]
public class CurrencyExchangeAmount
{
	/// <summary>
	/// <para>Conversion amount in decimal format</para>
	/// </summary>
	public decimal Amount { get; set; }

	/// <summary>
	/// <para>Currency code</para>
	/// </summary>
	public required Currency Currency { get; set; }

	/// <summary>
	/// <para>Target currency code</para>
	/// </summary>
	public FeePolicy FeePolicy { get; set; }
}
