using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests.Requests;
public class ReceiveRequestReq : RequestBase
{
	/// <summary>
	/// Bolt11 receive params.
	/// Send as an empty object to use a default configuration. If the object is not set at all, bolt11 invoice will not be created for the receive request.
	/// </summary>
	public Bolt11ReceiveRequestReq? Bolt11 { get; init; }

	/// <summary>
	/// Bolt12 receive params.
	/// Send as an empty object to use a default configuration. If the object is not set at all, bolt12 offer will not be created for the receive request.
	/// </summary>
	public Bolt12ReceiveRequestReq? Bolt12 { get; init; }

	/// <summary>
	/// Onchain receive params.
	/// Send as an empty object to use a default configuration. If the object is not set at all, onchain address will not be created for the receive request.
	/// </summary>
	public OnchainReceiveRequestReq? Onchain { get; init; }

	/// <summary>
	/// Currency to convert the received amounts to.
	/// Each receive for this request will be converted to the target currency using the current exchange rate. The amount credited can be different from the
	/// requested amount due to exchange rate fluctuations.
	/// </summary>
	/// <example>USD</example>
	public Currency? TargetCurrency { get; init; }
}
