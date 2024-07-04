using Strike.Client.PaymentQuotes.Lightning;
using Strike.Client.PaymentQuotes.Onchain;
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
				.ParseResponse<LnPaymentQuote>();

		/// <summary>
		/// Create LNURL/LN address payment quote
		/// <para>Endpoint accepts <c>idempotency-key</c> header to prevent the same quote from being created twice. In case of a duplicate request,
		/// the error code <c>DUPLICATE_PAYMENT_QUOTE</c> containing the id of the original quote (<c>paymentQuoteId</c>) will be returned.</para>
		/// </summary>
		public Task<LnPaymentQuote> CreateLnurlQuote(LnurlPaymentQuoteReq request) =>
			Client.Post("/v1/payment-quotes/lightning/lnurl", request)
				.ParseResponse<LnPaymentQuote>();

		/// <summary>
		/// Get LNURL details
		/// </summary>
		public Task<LnurlDetails> GetLnurlDetails(string lnAddressOrUrl) =>
			Client.Get($"/v1/payment-quotes/lightning/lnurl/{lnAddressOrUrl}")
				.ParseResponse<LnurlDetails>();

		/// <summary>
		/// Create onchain payment quote
		/// <para>Endpoint accepts <c>idempotency-key</c> header to prevent the same quote from being created twice. In case of a duplicate request,
		/// the error code <c>DUPLICATE_PAYMENT_QUOTE</c> containing the id of the original quote (<c>paymentQuoteId</c>) will be returned.</para>
		/// </summary>
		public Task<OnchainPaymentQuote> CreateOnchainQuote(OnchainPaymentQuoteReq request) =>
			Client.Post("/v1/payment-quotes/onchain", request)
				.ParseResponse<OnchainPaymentQuote>();

		/// <summary>
		/// Get available onchain tiers
		/// </summary>
		public Task<ResponseCollection<OnchainTier>> GetOnchainTiers(OnchainTiersReq request) =>
			Client.Post("/v1/payment-quotes/onchain/tiers", request)
				.ParseResponse<ResponseCollection<OnchainTier>>();

		/// <summary>
		/// Execute payment quote and send bitcoin
		/// </summary>
		public Task<Payment> ExecuteQuote(Guid quoteId) =>
			Client.Patch($"/v1/payment-quotes/{quoteId}/execute")
				.ParseResponse<Payment>();
	}
}
