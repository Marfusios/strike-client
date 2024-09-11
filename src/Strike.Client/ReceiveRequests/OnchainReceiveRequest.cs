using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("Onchain {Address}")]
public class OnchainReceiveRequest
{
	/// <summary>
	/// Onchain address to be paid to.
	/// </summary>
	/// <example>3sfdel53xhl59fn0d4nvdry5ut4zygy0</example>
	public required string Address { get; init; }

	/// <summary>
	/// Requested amount.
	/// </summary>
	/// <example>{ "amount": "10.00", "currency": "USD" }</example>
	public Money? RequestedAmount { get; init; }

	/// <summary>
	/// Amount in BTC suggested to be paid.
	/// This is a requested amount converted to BTC based on the current exchange rate. If the requested amount is already in BTC the values will be the same.
	/// </summary>
	/// <example>{ "amount": "0.00017", "currency": "BTC" }</example>
	public decimal BtcAmount { get; init; }
}
