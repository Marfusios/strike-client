using Strike.Client.Payments;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Payments API endpoints
	/// </summary>
	public PaymentsClient Payments => new(this);

	/// <summary>
	/// Payments API endpoints
	/// </summary>
	public record PaymentsClient(StrikeClient Client)
	{
		/// <summary>
		/// Find payment by id 
		/// </summary>
		public Task<Payment> FindPayment(Guid paymentId) =>
			Client.Get($"/v1/payments/{paymentId}")
				.ParseResponse<Payment>();
	}
}
