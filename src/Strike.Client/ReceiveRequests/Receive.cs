using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("Receive {Type} received: {AmountReceived}, credited: {AmountCredited} | {State}")]
public record Receive
{
	/// <summary>
	/// ID of the receive.
	/// </summary>
	public Guid ReceiveId { get; init; }

	/// <summary>
	/// Type of the receive.
	/// </summary>
	public ReceiveType Type { get; init; }

	/// <summary>
	/// State of the receive.
	/// </summary>
	public ReceiveState State { get; init; }

	/// <summary>
	/// Amount received.
	/// In case of LIGHTNING and ONCHAIN receives, this is always the Bitcoin amount.
	/// </summary>
	/// <example>{ "amount": "0.001", "currency": "BTC" }</example>
	public required Money AmountReceived { get; init; }

	/// <summary>
	/// Amount credited to the balance of the receive request's target currency.
	/// When the currency received is different from the target currency, the amount received is converted to the target currency at the current exchange rate.
	/// Will be present only for the completed receives.
	/// </summary>
	/// <example>{ "amount": "0.001", "currency": "BTC" }</example>
	public Money? AmountCredited { get; init; }

	/// <summary>
	/// Conversion rate applied to convert the received currency to the target currency.
	/// </summary>
	public ConversionAmount? ConversionRate { get; init; }

	/// <summary>
	/// Time at which the receive was detected and created in the system.
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// Time at which the receive was completed and credited to the balance.
	/// </summary>
	public DateTimeOffset? Completed { get; init; }

	/// <summary>
	/// Onchain data. Will be present only for onchain receives.
	/// </summary>
	public OnchainData? Onchain { get; init; }

	/// <summary>
	/// Onchain data. Will be present only for lighting receives.
	/// </summary>
	public LightningData? Lightning { get; init; }

	/// <summary>
	/// P2P data. Will be present only for P2P receives.
	/// </summary>
	[JsonPropertyName("p2p")]
	public P2PData? P2P { get; init; }

	[DebuggerDisplay("Onchain {Address} confirmations: {NumberOfConfirmations}")]
	public record OnchainData
	{
		/// <summary>
		/// Bitcoin address to which the onchain payment was made.
		/// </summary>
		public required string Address { get; init; }

		/// <summary>
		/// Transaction ID of the onchain payment.
		/// </summary>
		public required string TransactionId { get; init; }

		/// <summary>
		/// The height of the block in which the transaction was included.
		/// </summary>
		public ulong? BlockHeight { get; init; }

		/// <summary>
		/// Number of confirmations of the transaction.
		/// </summary>
		public ulong? NumberOfConfirmations { get; init; }
	}

	[DebuggerDisplay("Lightning {Description} hash: {PaymentHash}")]
	public record LightningData
	{
		/// <summary>
		/// Lightning invoice.
		/// </summary>
		public required string Invoice { get; init; }

		/// <summary>
		/// Payment preimage.
		/// </summary>
		public required string Preimage { get; init; }

		/// <summary>
		/// Description of the payment.
		/// </summary>
		public string? Description { get; init; }

		/// <summary>
		/// Description hash of the payment.
		/// </summary>
		public string? DescriptionHash { get; init; }

		/// <summary>
		/// Payment hash of the payment.
		/// </summary>
		public required string PaymentHash { get; init; }
	}

	[DebuggerDisplay("P2P from: {PayerAccountId}")]
	public record P2PData
	{
		/// <summary>
		/// ID of the account that made the payment.
		/// </summary>
		public Guid PayerAccountId { get; init; }
	}
}
