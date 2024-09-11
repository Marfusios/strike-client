using Strike.Client.Models;

namespace Strike.Client.ReceiveRequests.Requests;
public class Bolt11ReceiveRequestReq
{
	/// <summary>
	/// Amount to be used for the lightning invoice. If not set, zero amount invoice will be created.
	/// If currency is not BTC, the amount will be converted to BTC using the current exchange rate.
	/// </summary>
	/// <example>{ "amount": "10.00", "currency": "USD" }</example>
	public Money? Amount { get; init; }

	/// <summary>
	/// Lightning invoice description.
	/// </summary>
	/// <example>For pizza</example>
	public string? Description { get; init; }

	/// <summary>
	/// Lightning invoice description hash hex string. If provided, the invoice description will be ignored in favor of this hash
	/// </summary>
	/// <example>3925b6f67e2c340036ed12093dd44e0368df1b6ea26c53dbe4811f58fd5db8c1</example>
	public string? DescriptionHash { get; init; }

	/// <summary>
	/// Lightning invoice expiry in seconds
	/// </summary>
	/// <example>60</example>
	public ulong? ExpiryInSeconds { get; init; }
}
