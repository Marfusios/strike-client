using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Deposits;

[DebuggerDisplay("DepositFeeEstimate {Total}, {FeeFixed} + {FeePercent} | {SettledAt} {SettlementPeriodInDay}")]
public record DepositFeeEstimate : ResponseBase
{
	/// <summary>
	/// Amount to be deposited
	/// </summary>
	public Money? Amount { get; init; }

	/// <summary>
	/// The amount to be charged as the fee for the deposit.
	/// </summary>
	public Money? Fee { get; init; }

	/// <summary>
	/// The total amount to be deposited from the payment method.
	/// </summary>
	public Money? Total { get; init; }

	/// <summary>
	/// The fee percentage, if applicable
	/// </summary>
	public string? FeePercent { get; init; }

	/// <summary>
	/// The fixed fee, if applicable
	/// </summary>
	public string? FeeFixed { get; init; }

	/// <summary>
	/// Timestamp at which the settlement will happen. At this time, the funds become available to be spent or sent out of the system (see balance endpoint for more information). If null, the settlement is immediate
	/// </summary>
	/// <remarks>Returns the default timestamp for immediate settlement. Use <see cref="SettlementTime"/> to distinguish it.</remarks>
	[JsonIgnore]
	public DateTimeOffset SettledAt
	{
		get => SettlementTime.GetValueOrDefault();
		init => SettlementTime = value;
	}

	/// <summary>
	/// Settlement timestamp, or null when the funds settle immediately.
	/// </summary>
	[JsonPropertyName("settledAt")]
	public DateTimeOffset? SettlementTime { get; init; }

	/// <summary>
	/// Settlement period in days. Deprecated by Strike in favor of the settlement timestamp. If null, settlement is immediate.
	/// </summary>
	public int? SettlementPeriodInDay { get; init; }
}
