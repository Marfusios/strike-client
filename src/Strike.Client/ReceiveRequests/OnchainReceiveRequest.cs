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
	/// Bitcoin URI containing the address and optional payment amount.
	/// </summary>
	public string AddressUri { get; init; } = default!;

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
	/// <remarks>Returns zero when no amount was requested. Use <see cref="RequestedBtcAmount"/> to distinguish an absent amount.</remarks>
	[JsonIgnore]
	public decimal BtcAmount
	{
		get => RequestedBtcAmount.GetValueOrDefault();
		init => RequestedBtcAmount = value;
	}

	/// <summary>
	/// Suggested bitcoin amount, or null when no amount was requested.
	/// </summary>
	[JsonPropertyName("btcAmount")]
	public decimal? RequestedBtcAmount { get; init; }
}
