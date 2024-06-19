using Strike.Client.PaymentQuotes;

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
		/// <para>Endpoint accepts <c>idempotency-key</c> header to prevent the same quote from being created twice. In case of a duplicate request,
		/// the error code <c>DUPLICATE_PAYMENT_QUOTE</c> containing the id of the original quote (<c>paymentQuoteId</c>) will be returned.</para>
		/// </summary>
		public Task<LnPaymentQuote> CreateLnurlQuote(CreateLnurlPaymentQuoteReq request) =>
			Client.PostAsync("/v1/payment-quotes/lightning/lnurl", request)
				.ParseResponseAsync<LnPaymentQuote>();
	}
}
