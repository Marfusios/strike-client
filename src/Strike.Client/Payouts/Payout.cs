using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Payouts;

/// <summary>
/// A payout to a bank payment method.
/// </summary>
[DebuggerDisplay("Payout {Id}: {Amount} | {State}")]
public record Payout : ResponseBase
{
	/// <summary>
	/// Payout ID.
	/// </summary>
	public Guid Id { get; init; }

	/// <summary>
	/// Current payout state.
	/// </summary>
	public PayoutState State { get; init; }

	/// <summary>
	/// Time the payout was created.
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// ID of the payment method used for this payout.
	/// </summary>
	public Guid PaymentMethodId { get; init; }

	/// <summary>
	/// ID of the actual source of funds, when a payout originator is supplied.
	/// </summary>
	public Guid? OriginatorId { get; init; }

	/// <summary>
	/// Amount paid out, excluding the fee. Total funds spent are the amount plus the fee.
	/// </summary>
	public Money Amount { get; init; } = null!;

	/// <summary>
	/// Fee charged for the payout.
	/// </summary>
	public PayoutFee? Fee { get; init; }

	/// <summary>
	/// Reference passed to the receiver.
	/// </summary>
	public string? Reference { get; init; }

	/// <summary>
	/// Time the payout was initiated, if available.
	/// </summary>
	public DateTimeOffset? Initiated { get; init; }

	/// <summary>
	/// Time funds were sent to the receiver. The receiving bank may take additional time to process them.
	/// </summary>
	public DateTimeOffset? Completed { get; init; }

	/// <summary>
	/// Transaction ID for reporting and account statements.
	/// </summary>
	public Guid? TransactionId { get; init; }
}
