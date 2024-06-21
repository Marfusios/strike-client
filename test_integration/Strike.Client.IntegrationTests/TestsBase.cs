using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Strike.Client.IntegrationTests;

public class TestsBase
{
	protected ServiceProvider Provider { get; }

	public TestsBase()
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.Tests.json", optional: false)
			.AddEnvironmentVariables()
			.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
			.Build();

		Provider = new ServiceCollection()
			.AddStrike(config)
			.BuildServiceProvider();
	}

	protected StrikeClient GetClient()
	{
		var client = Provider.GetRequiredService<StrikeClient>();
		Skip.IfNot(IsApiKeySet(client), "ApiKey is not set, skip tests");
		return client;
	}

	protected static void AssertStatus(ResponseBase response)
	{
		Assert.True(response.IsSuccessStatusCode, $"Error status: {(int)response.StatusCode} {response.StatusCode}");
	}

	protected static bool IsApiKeySet(StrikeClient client) =>
		!string.IsNullOrWhiteSpace(client.ApiKey) && client.ApiKey != "YOUR_API_KEY";
}
