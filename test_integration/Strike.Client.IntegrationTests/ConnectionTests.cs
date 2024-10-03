namespace Strike.Client.IntegrationTests;
public class ConnectionTests : TestsBase
{
	[SkippableFact]
	public async Task InvalidUrl_ShouldNotThrowException()
	{
		var client = GetClient();
		client.ThrowOnError = false;
		client.ServerUrl = new Uri("https://localhost:1123");

		var response = await client.Balances.GetBalances();

		Assert.False(response.IsSuccessStatusCode);
	}
}
