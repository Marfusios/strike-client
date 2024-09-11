using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("Bolt11 {Description} amount: {BtcAmount}/{RequestedAmount}, expires: {Expires}")]
public record Bolt11ReceiveRequest
{
	/// <summary>
	/// BOLT 11 lightning invoice.
	/// </summary>
	public required string Invoice { get; init; }

	/// <summary>
	/// Requested invoice amount.
	/// </summary>
	/// <example>{ "amount": "10.00", "currency": "USD" }</example>
	public Money? RequestedAmount { get; init; }

	/// <summary>
	/// Invoice amount in BTC.
	/// This is a requested amount converted to BTC based on the current exchange rate. If the requested amount is already in BTC the values will be the same.
	/// </summary>
	/// <example>0.00017</example>
	public decimal BtcAmount { get; init; }

	/// <summary>
	/// Invoice description.
	/// </summary>
	/// <example>For pizza</example>
	public string? Description { get; init; }

	/// <summary>
	/// Invoice description hash hex string.
	/// </summary>
	/// <example>3925b6f67e2c340036ed12093dd44e0368df1b6ea26c53dbe4811f58fd5db8c1</example>
	public string? DescriptionHash { get; init; }

	/// <summary>
	/// Invoice payment hash hex string.
	/// </summary>
	/// <example>4215b6f67e2c340036ed12093dd44e0368df1b6ea26c53dbe4811f58fd5db8c1</example>
	public required string PaymentHash { get; init; }

	/// <summary>
	/// Time of invoice expiration.
	/// </summary>
	public DateTimeOffset Expires { get; init; }
}
