using Strike.Client.Models;

namespace Strike.Client.Payouts;

/// <summary>
/// Request to create a payout.
/// </summary>
public class PayoutReq : RequestBase
{
	/// <summary>
	/// Payment method to use. It determines the payout currency.
	/// </summary>
	public required Guid PaymentMethodId { get; init; }

	/// <summary>
	/// Actual source of funds. Omit when the Strike account owner is the sender.
	/// </summary>
	public Guid? OriginatorId { get; init; }

	/// <summary>
	/// Amount to pay out in the payment method's currency, as a decimal string.
	/// </summary>
	public required string Amount { get; init; }

	/// <summary>
	/// Reference passed to the receiver. Length limits depend on the payment method transfer type.
	/// </summary>
	public string? Reference { get; init; }

	/// <summary>
	/// Whether the fee is included in the amount or added on top. Strike defaults to EXCLUSIVE.
	/// </summary>
	public FeePolicy? FeePolicy { get; init; }
}
