using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Deposits;

[DebuggerDisplay("Deposit {TotalAmount} --> {Amount} | {State}")]
public record Deposit : ResponseBase
{
	/// <summary>
	/// The deposit's ID
	/// </summary>
	/// <example>
	/// 99bb499a-aa28-4bf6-b976-4b74b2bee31f
	/// </example>
	public Guid Id { get; init; }

	/// <summary>
	/// Amount of the deposit
	/// </summary>
	public Money? Amount { get; init; }

	/// <summary>
	/// The amount to be charged as the fee for the deposit.
	/// </summary>
	public Money? Fee { get; init; }

	/// <summary>
	/// The total amount to be deposited from the payment method.
	/// </summary>
	public Money? TotalAmount { get; init; }

	/// <summary>
	/// The state of the deposit process. The deposit can be created in either New or Pending state.
	/// </summary>
	public DepositState State { get; init; }

	/// <summary>
	/// The reason for the deposit failure.
	/// </summary>
	public string? FailureReason { get; init; }
}
