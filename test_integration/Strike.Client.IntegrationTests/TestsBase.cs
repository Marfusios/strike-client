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
		var msg =
			$"Error status: {(int)response.StatusCode} {response.StatusCode} ({response.Error?.Data.Code} {response.Error?.Data.Message})";
		if (response.Error?.Data.ValidationErrors?.Any() == true)
		{
			var props = response.Error.Data.ValidationErrors.Select(x =>
				$"{x.Key}: {string.Join(", ", x.Value.Select(y => $"{y.Code} - {y.Message}"))}");
			msg += " Validations: " + string.Join("; ", props);
		}
		Assert.True(response.IsSuccessStatusCode, msg);
	}

	protected static bool IsApiKeySet(StrikeClient client) =>
		!string.IsNullOrWhiteSpace(client.ApiKey) && client.ApiKey != "YOUR_API_KEY";
}
