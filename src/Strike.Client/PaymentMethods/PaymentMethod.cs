using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.PaymentMethods;

[DebuggerDisplay("Payment Method {TransferType} {Id} | {State}")]
public record PaymentMethod : ResponseBase
{
	/// <summary>
	/// The payment methods's ID
	/// </summary>
	/// <example>
	/// 99bb499a-aa28-4bf6-b976-4b74b2bee31f
	/// </example>
	public Guid Id { get; init; }

	/// <summary>
	/// State of the payment method
	/// </summary>
	public PaymentMethodState State { get; init; }

	/// <summary>
	/// State of the payment method
	/// </summary>
	public string[]? SupportedActions { get; init; }

	/// <summary>
	/// The timestamp of the payment completion
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// State of the payment method
	/// </summary>
	public PaymentMethodTransferTypes TransferType { get; init; }

	/// <summary>
	/// Account Number
	/// </summary>
	public string? AccountNumber { get; init; }

	/// <summary>
	/// Routing Number
	/// </summary>
	public string? RoutingNumber { get; init; }

	/// <summary>
	/// Reference Code
	/// </summary>
	public string? ReferenceCode { get; init; }

	/// <summary>
	/// Account Type
	/// </summary>
	/// <remarks>Returns Checking when no account type is provided. Use <see cref="OptionalAccountType"/> to distinguish an absent value.</remarks>
	[JsonIgnore]
	public PaymentMethodAccountTypes AccountType
	{
		get => OptionalAccountType.GetValueOrDefault();
		init => OptionalAccountType = value;
	}

	/// <summary>
	/// Bank account type, when supplied by Strike.
	/// </summary>
	[JsonPropertyName("accountType")]
	public PaymentMethodAccountTypes? OptionalAccountType { get; init; }

	/// <summary>
	/// Currency of the bank account.
	/// </summary>
	public Currency Currency { get; init; }

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
	public Beneficiary[]? Beneficiaries { get; init; }
}
