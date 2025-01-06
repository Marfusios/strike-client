using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.PaymentMethods;

[DebuggerDisplay("Payment Method Request {TransferType} {BankName}")]
public class PaymentMethodReq : RequestBase
{
	/// <summary>
	/// State of the payment method
	/// </summary>
	public required PaymentMethodTransferTypes TransferType { get; init; }

	/// <summary>
	/// Account Number
	/// </summary>
	public required string AccountNumber { get; init; }

	/// <summary>
	/// Routing Number
	/// </summary>
	public required string RoutingNumber { get; init; }

	/// <summary>
	/// Account Type
	/// </summary>
	public PaymentMethodAccountTypes AccountType { get; init; }

	/// <summary>
	/// Bank Name
	/// </summary>
	public string? BankName { get; init; }

	/// <summary>
	/// Bank Name
	/// </summary>
	public Address? BankAddress { get; init; }

	/// <summary>
	/// Bank Name
	/// </summary>
	public required Beneficiary[] Beneficiaries { get; init; }
}
