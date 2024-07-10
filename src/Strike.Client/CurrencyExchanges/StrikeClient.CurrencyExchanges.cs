using Strike.Client.CurrencyExchanges;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Currency Exchanges API endpoints
	/// </summary>
	public CurrencyExchangesClient CurrencyExchanges => new(this);

	/// <summary>
	/// Currency Exchanges API endpoints
	/// </summary>
	public record CurrencyExchangesClient(StrikeClient Client)
	{
		/// <summary>
		/// Get a quote to exchange currencies from one to another
		/// </summary>
		public Task<CurrencyExchangeQuote> PostCurrencyExchangeQuote(CurrencyExchangeQuoteReq req) =>
			Client.Post("/v1/currency-exchange-quotes", req)
				.ParseResponse<CurrencyExchangeQuote>();

		/// <summary>
		/// Execute currency exchange
		/// </summary>
		public Task<CurrencyExchangeExecute> PatchExecuteQuote(Guid quoteId) =>
			Client.Patch($"/v1/currency-exchange-quotes/{quoteId}/execute")
				.ParseResponse<CurrencyExchangeExecute>();

		/// <summary>
		/// Get currency exchange quote by id
		/// </summary>
		public Task<CurrencyExchangeQuote> GetCurrencyExchangeQuote(Guid quoteId) =>
			Client.Get($"/v1/currency-exchange-quotes/{quoteId}")
				.ParseResponse<CurrencyExchangeQuote>();
	}
}