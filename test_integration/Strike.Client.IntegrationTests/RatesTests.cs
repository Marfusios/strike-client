namespace Strike.Client.IntegrationTests;
public class RatesTests : TestsBase
{
	[SkippableFact]
	public async Task GetRates_ShouldWork()
	{
		var client = GetClient();

		var rates = await client.Rates.GetRatesTicker();

		AssertStatus(rates);
		Assert.NotEmpty(rates);
	}
}
