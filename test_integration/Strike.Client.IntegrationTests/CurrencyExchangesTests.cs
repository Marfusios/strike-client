using Strike.Client.CurrencyExchanges;
using Strike.Client.Models;

namespace Strike.Client.IntegrationTests;
public class CurrencyExchangesTests : TestsBase
{
	[SkippableFact]
	public async Task CurrencyExchangeQuote_ShouldWork()
	{
		var client = GetClient();

		var exchangeQuote = await client.CurrencyExchanges.CreateQuote(new CurrencyExchangeQuoteReq
		{
			Sell = Currency.Btc,
			Buy = Currency.Usd,
			Amount = new MoneyWithFee
			{
				Amount = 0.000001m,
				Currency = Currency.Btc,
				FeePolicy = FeePolicy.Exclusive
			}
		});

		AssertStatus(exchangeQuote);

		var getQuote = await client.CurrencyExchanges.GetQuote(exchangeQuote.Id);

		AssertStatus(getQuote);

		var exec = await client.CurrencyExchanges.ExecuteQuote(exchangeQuote.Id);

		AssertStatus(exec);
	}

}
