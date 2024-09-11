using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests.Requests;
public class OnchainReceiveRequestReq
{
	/// <summary>
	/// Amount to be suggested to the payer.
	/// </summary>
	/// <example>{ "amount": "1000.00", "currency": "USD" }</example>
	public Money? Amount { get; init; }
}
