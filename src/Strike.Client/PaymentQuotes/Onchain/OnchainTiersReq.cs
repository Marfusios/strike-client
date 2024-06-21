using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes.Onchain;
public class OnchainTiersReq : RequestBase
{
	/// <summary>
	/// A valid Bitcoin address
	/// </summary>
	/// <example>bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh</example>
	public required string BtcAddress { get; init; }

	/// <summary>
	/// Amount to send
	/// </summary>
	public required Money Amount { get; init; }
}
