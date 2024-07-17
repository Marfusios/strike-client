using Strike.Client.CurrencyExchanges;
using Strike.Client.Models;

namespace Strike.Client.IntegrationTests;
public class CurrencyExchangesTests : TestsBase
{
	[SkippableFact]
	public async Task CurrencyExchangeQuote_ShouldWork()
	{
		var client = GetClient();

		var exchangeQuote = await client.CurrencyExchanges.PostCurrencyExchangeQuote(new CurrencyExchangeQuoteReq
		{
			Sell = Currency.Btc,
			Buy = Currency.Usd,
			Amount = new CurrencyExchangeAmount
			{
				Amount = 0.000001m,
				Currency = Currency.Btc,
				FeePolicy = FeePolicy.Exclusive
			}
		});
		
		AssertStatus(exchangeQuote);

		var getQuote = await client.CurrencyExchanges.GetCurrencyExchangeQuote(exchangeQuote.Id);
		
		AssertStatus(getQuote);

		var exec = await client.CurrencyExchanges.PatchExecuteQuote(exchangeQuote.Id);
		
		AssertStatus(exec);
	}

}
