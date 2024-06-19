using Strike.Client.Models;

namespace Strike.Client.Accounts;

/// <summary>
/// Account balance summary
/// </summary>
public record AccountCurrency
{
	/// <summary>Currency code</summary>
	public Currency Currency { get; init; }

	/// <summary>Signals the default currency for the account</summary>
	/// <example>true</example>
	public bool IsDefaultCurrency { get; init; }

	/// <summary>Signals if the currency is the available for the account</summary>
	/// <example>true</example>
	public bool IsAvailable { get; init; }

	/// <summary>Signals if the account can issue invoices in this currency</summary>
	/// <example>true</example>
	public bool IsInvoiceable { get; init; }
}
