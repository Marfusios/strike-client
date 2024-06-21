using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Payments;

[DebuggerDisplay("Payment {TotalAmount} --> {Amount} | {State}")]
public record Payment : ResponseBase
{
	/// <summary>
	/// The payment's ID
	/// </summary>
	/// <example>
	/// 99bb499a-aa28-4bf6-b976-4b74b2bee31f
	/// </example>
	public Guid PaymentId { get; init; }

	/// <summary>
	/// Status of the payment execution
	/// </summary>
	public PaymentState State { get; init; }

	/// <summary>
	/// The timestamp of the payment completion
	/// </summary>
	public DateTimeOffset? Completed { get; init; }

	/// <summary>
	/// The conversion rate details
	/// </summary>
	public ConversionAmount? ConversionRate { get; init; }

	/// <summary>
	/// The amount that the receiver will receive in sender’s currency
	/// </summary>
	public Money Amount { get; init; } = default!;

	/// <summary>
	/// The total of all fees
	/// </summary>
	public Money? TotalFee { get; init; }

	/// <summary>
	/// The fee required by LN network. Only applicable for LN payments
	/// </summary>
	public Money? LightningNetworkFee { get; init; }

	/// <summary>
	/// The amount that the sender will spend
	/// </summary>
	public Money TotalAmount { get; init; } = default!;

	/// <summary>
	/// The reward that the sender might receive, if applicable
	/// </summary>
	public Money? Reward { get; init; }
}
