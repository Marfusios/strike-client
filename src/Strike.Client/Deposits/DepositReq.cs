using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Deposits;

[DebuggerDisplay("Deposit Request {PaymentMethodId} --> {Amount} | {Fee}")]
public class DepositReq : RequestBase
{
	/// <summary>
	/// Payment method to deposit.
	/// </summary>
	public required Guid PaymentMethodId { get; init; }

	/// <summary>
	/// Amount to deposit. Currency is defined by the payment method.
	/// </summary>
	public required Money Amount { get; init; }

	/// <summary>
	/// Should the fee be included in the amount or added on top of it. Defaults to EXCLUSIVE.
	/// </summary>
	public FeePolicy? Fee { get; init; }
}
