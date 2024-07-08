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
		/// Get account balance details
		/// </summary>
		public Task<CurrencyExchangeQuote> PostCurrencyExchangeQuotes(CurrencyExchangeQuoteReq req) =>
			Client.Post("/v1/currency-exchange-quotes", req)
				.ParseResponse<CurrencyExchangeQuote>();
	}
}