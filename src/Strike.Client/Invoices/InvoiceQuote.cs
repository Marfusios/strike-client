using Strike.Client.Models;

namespace Strike.Client.Invoices;
public record InvoiceQuote : ResponseBase
{
	/// <summary>
	/// Quote unique identifier
	/// </summary>
	public Guid QuoteId { get; init; }

	/// <summary>
	/// Result state of the quote. Represents the final state of the quote (either `EXPIRED` or `PAID`).
	/// Until it reaches the final state, it will be in `PENDING` state.
	/// </summary>
	/// <example>PENDING</example>
	[JsonIgnore]
	public InvoiceQuoteResult Result { get; init; }

	/// <summary>
	/// Quote description
	/// </summary>
	/// <example>Pizza and beer</example>
	public string? Description { get; init; }

	/// <summary>
	/// Bolt 11 encoded lightning invoice
	/// </summary>
	public string LnInvoice { get; init; } = default!;

	/// <summary>
	/// On-chain address that can be used to pay the invoice. Will be present only if you are eligible for on-chain payments
	/// </summary>
	/// <example>3sfdel53xhl59fn0d4nvdry5ut4zygy0</example>
	public string? OnchainAddress { get; init; }

	/// <summary>
	/// Time at which quote is expiring
	/// </summary>
	public DateTimeOffset Expiration { get; init; }

	/// <summary>
	/// Time period in seconds for which quote is valid
	/// </summary>
	/// <example>60</example>
	public long ExpirationInSec { get; init; }

	/// <summary>
	/// Quote amount in currency that will be credited. This currency cannot be BTC
	/// </summary>
	/// <example>{ "currency": "USD", "amount": "10.99" }</example>
	public Money TargetAmount { get; init; } = default!;

	/// <summary>
	/// Quote amount in currency that will be debited. At quote creation time this will always be BTC, but that can change to other currencies later in case
	/// the transaction is upgraded to direct transfer (happens if the payer is also Strike user)
	/// </summary>
	/// <example>{ "currency": "BTC", "amount": "0.00023563" }</example>
	public Money SourceAmount { get; init; } = default!;

	/// <summary>
	/// Applied source -> target currency conversion rate
	/// </summary>
	public ConversionAmount ConversionRate { get; init; } = default!;
}
