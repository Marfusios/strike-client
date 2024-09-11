using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("ReceiveRequest {TargetCurrency.ToString()} {ReceiveRequestId}")]
public record ReceiveRequest : ResponseBase
{
	/// <summary>
	/// ID of the receive request.
	/// </summary>
	public Guid ReceiveRequestId { get; init; }

	/// <summary>
	/// Time of receive request creation.
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// Currency to which each receive for this request will be converted.
	/// Conversion will be done using the current exchange rate at the time of the receive. The amount credited can be different from the requested amount due
	/// to exchange rate fluctuations.
	/// </summary>
	/// <example>USD</example>
	public Currency? TargetCurrency { get; init; }

	/// <summary>
	/// Bolt11 receive request.
	/// </summary>
	public Bolt11ReceiveRequest? Bolt11 { get; init; }

	/// <summary>
	/// Bolt12 receive request.
	/// </summary>
	public Bolt12ReceiveRequest? Bolt12 { get; init; }

	/// <summary>
	/// Onchain receive request.
	/// </summary>
	public OnchainReceiveRequest? Onchain { get; init; }
}
