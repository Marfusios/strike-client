using Strike.Client.PaymentQuotes;
using Strike.Client.Payments;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Payment Quotes API endpoints
	/// </summary>
	public PaymentQuotesClient PaymentQuotes => new(this);

	/// <summary>
	/// Payment Quotes API endpoints
	/// </summary>
	public record PaymentQuotesClient(StrikeClient Client)
	{
		/// <summary>
		/// Create lightning payment quote
		/// <para>Endpoint accepts <c>idempotency-key</c> header to prevent the same quote from being created twice. In case of a duplicate request,
		/// the error code <c>DUPLICATE_PAYMENT_QUOTE</c> containing the id of the original quote (<c>paymentQuoteId</c>) will be returned.</para>
		/// </summary>
		public Task<LnPaymentQuote> CreateLnQuote(LnPaymentQuoteReq request) =>
			Client.Post("/v1/payment-quotes/lightning", request)
				.ParseResponseAsync<LnPaymentQuote>();

		/// <summary>
		/// Create LNURL/LN address payment quote
		/// <para>Endpoint accepts <c>idempotency-key</c> header to prevent the same quote from being created twice. In case of a duplicate request,
		/// the error code <c>DUPLICATE_PAYMENT_QUOTE</c> containing the id of the original quote (<c>paymentQuoteId</c>) will be returned.</para>
		/// </summary>
		public Task<LnPaymentQuote> CreateLnurlQuote(LnurlPaymentQuoteReq request) =>
			Client.Post("/v1/payment-quotes/lightning/lnurl", request)
				.ParseResponseAsync<LnPaymentQuote>();

		/// <summary>
		/// Execute payment quote and send bitcoin
		/// </summary>
		public Task<Payment> ExecuteQuote(Guid quoteId) =>
			Client.Patch($"/v1/payment-quotes/{quoteId}/execute")
				.ParseResponseAsync<Payment>();
	}
}
