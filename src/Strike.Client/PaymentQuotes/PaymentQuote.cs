using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes;
public record PaymentQuote : ResponseBase
{
	/// <summary>
	/// <para>The payment quote's ID</para>
	/// </summary>
	public Guid PaymentQuoteId { get; init; }

	/// <summary>
	/// <para>The description forwarded from the related invoice</para>
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// <para>The expiry timestamp</para>
	/// </summary>
	public DateTimeOffset? ValidUntil { get; init; }

	/// <summary>
	/// <para>The conversion rate details</para>
	/// </summary>
	public ConversionAmount? ConversionRate { get; init; }

	/// <summary>
	/// <para>The amount that the receiver will receive in sender’s currency</para>
	/// </summary>
	public Money Amount { get; init; } = default!;

	/// <summary>
	/// <para>The total of all fees</para>
	/// </summary>
	public Money? TotalFee { get; init; }

	/// <summary>
	/// <para>The amount that the sender will spend</para>
	/// </summary>
	public Money TotalAmount { get; init; } = default!;

	/// <summary>
	/// <para>The reward that the sender might receive, if applicable</para>
	/// </summary>
	public Money? Reward { get; init; }
}
