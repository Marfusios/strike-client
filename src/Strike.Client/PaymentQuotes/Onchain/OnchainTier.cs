using Strike.Client.Models;

namespace Strike.Client.PaymentQuotes.Onchain;
public record OnchainTier
{
	/// <summary>
	/// The ID of the tier. Used in the onchain payment quote creation.
	/// </summary>
	/// <example>tier_fast</example>
	public string Id { get; init; } = default!;

	/// <summary>
	/// Estimated delivery duration in minutes.
	/// </summary>
	/// <example>10</example>
	public uint EstimatedDeliveryDurationInMin { get; init; }

	/// <summary>
	/// Estimated fee for the transaction.
	/// </summary>
	/// <example>{ "currency": "BTC", "amount": "0.00023563" }</example>
	public Money EstimatedFee { get; init; } = default!;

	/// <summary>
	/// The minimum amount allowed to be sent with this tier
	/// </summary>
	/// <example>{ "currency": "BTC", "amount": "0.001" }</example>
	public Money? MinimumAmount { get; init; }
}
