using Strike.Client.PaymentMethods;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Payments API endpoints
	/// </summary>
	public PaymentMethodsClient PaymentMethods => new(this);

	/// <summary>
	/// Payments API endpoints
	/// </summary>
	public record PaymentMethodsClient(StrikeClient Client)
	{
		/// <summary>
		/// Create a new payment method
		/// </summary>
		public Task<PaymentMethod> Create(PaymentMethodReq request) =>
			Client.Post($"/v1/payment-methods/bank", request)
				.ParseResponse<PaymentMethod>();
		
		/// <summary>
		/// Get all payment methods
		/// </summary>
		public Task<PaymentMethodsCollection> GetPaymentMethods(
			int top = 100,
			int skip = 0,
			string? supportedAction = null
			)
		{
			var urlParams = ConstructUrlParams(
				(nameof(top), top),
				(nameof(skip), skip),
				(nameof(supportedAction), supportedAction)
			);
			return Client.Get($"/v1/payment-methods/bank{urlParams}")
				.ParseResponse<PaymentMethodsCollection>();
		}
		
		
		/// <summary>
		/// Find payment method by id 
		/// </summary>
		public Task<PaymentMethod> FindPaymentMethod(Guid id) =>
			Client.Get($"/v1/payment-methods/bank/{id}")
				.ParseResponse<PaymentMethod>();

		/// <summary>
		/// Delete payment method by id 
		/// </summary>
		public Task<PaymentMethod> DeletePaymentMethod(Guid id) =>
			Client.Delete($"/v1/payment-methods/bank/{id}")
				.ParseResponse<PaymentMethod>();
	}
}
