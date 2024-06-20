using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Balances;

/// <summary>
/// Current account balance for the target currency
/// </summary>
[DebuggerDisplay("Balance {Available} {Currency} (outgoing: {Outgoing})")]
public sealed record Balance : ResponseBase
{
	/// <summary>
	/// The currency of the balance
	/// </summary>
	/// <example>USD</example>
	public Currency Currency { get; init; }

	/// <summary>
	/// The balance currently being withdrawn
	/// </summary>
	/// <example>50.00</example>
	public decimal Outgoing { get; init; }

	/// <summary>
	/// The balance available for use
	/// </summary>
	/// <example>100.00</example>
	public decimal Available { get; init; }

	/// <summary>
	/// The total sum of the balance
	/// </summary>
	/// <example>150.00</example>
	public decimal Total { get; init; }
}
