using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Strike.Client.Testing;

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

		var services = new ServiceCollection().AddStrike(config);
		_ = services.AddHttpClient(StrikeOptions.HttpClientName)
			.AddHttpMessageHandler(() => new ReadOnlyHttpMessageHandler());
		Provider = services.BuildServiceProvider();
	}

	protected StrikeClient GetClient(bool readOnly = false)
	{
		// Only reviewed read-only tests may access configured account credentials.
		// The HTTP handler also blocks writes if a read-only test changes later.
		Skip.IfNot(readOnly, "Disabled: integration credentials are restricted to reviewed read-only tests.");
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
