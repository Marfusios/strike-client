using Strike.Client.Models;

namespace Strike.Client.Payouts;

/// <summary>
/// Payout fee amount, currency, and policy.
/// </summary>
public class PayoutFee : Money
{
	/// <summary>
	/// Whether the fee is included in the requested amount or added on top.
	/// </summary>
	public FeePolicy? FeePolicy { get; init; }
}
