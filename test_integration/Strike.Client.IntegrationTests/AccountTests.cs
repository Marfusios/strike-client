namespace Strike.Client.IntegrationTests;
public class AccountTests : TestsBase
{
	[SkippableFact]
	public async Task GetProfile_ByHandle_ShouldWork()
	{
		var client = GetClient();

		var handle = "frank_test";
		var account = await client.Accounts.GetProfile(handle);

		AssertStatus(account);
		Assert.Equal(handle, account.Handle);
		Assert.NotNull(account.LnAddress);
	}

	[SkippableFact]
	public async Task GetBalances_ShouldWork()
	{
		var client = GetClient();

		var balances = await client.Balances.GetBalances();

		AssertStatus(balances);
		Assert.NotEmpty(balances);
	}
}
