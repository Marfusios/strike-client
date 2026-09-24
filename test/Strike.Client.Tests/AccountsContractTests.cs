using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using Strike.Client.Converters;
using Strike.Client.Models;

namespace Strike.Client.Tests;

public class AccountsContractTests
{
	[Fact]
	public async Task GetLimits_ParsesAmountsCountsAndPeriods()
	{
		using var handler = new Handler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/accounts/limits", request.RequestUri?.AbsolutePath);
			return """
				{"deposit":{"count":{"effective":3,"lifetime":{"limit":100,"used":5},"calendarYear":{"limit":20,"used":4},"cycles":[{"days":7,"limit":10,"used":2}]},"amount":{"effective":{"amount":"12.50","currency":"AUD"},"single":{"amount":"100","currency":"AUD"},"cycles":[{"days":30,"limit":{"amount":"500","currency":"AUD"},"used":null}]}},"achDeposit":null,"send":{},"withdrawal":{},"instant":{"amount":{"effective":{"amount":"0","currency":"BTC"}}}}
				""";
		});
		using var http = new HttpClient(handler);

		var response = await CreateClient(http).Accounts.GetLimits();

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(3, response.Deposit?.Count?.Effective);
		Assert.Equal(5, response.Deposit?.Count?.Lifetime?.Used);
		Assert.Equal(20, response.Deposit?.Count?.CalendarYear?.Limit);
		Assert.Equal(7, Assert.Single(response.Deposit!.Count!.Cycles!).Days);
		Assert.Equal(12.50m, response.Deposit.Amount?.Effective.Amount);
		Assert.Equal(Currency.Aud, response.Deposit.Amount?.Effective.Currency);
		Assert.Null(Assert.Single(response.Deposit.Amount!.Cycles!).Used);
		Assert.Null(response.AchDeposit);
		Assert.NotNull(response.Send);
		Assert.NotNull(response.Withdrawal);
		Assert.Equal(0m, response.Instant?.Amount?.Effective.Amount);
	}

	[Fact]
	public async Task ProfileHandle_IsEscapedAsOnePathSegment()
	{
		using var handler = new Handler(request =>
		{
			Assert.Equal("/v1/accounts/handle/name%2Fwith%3Freserved%23text/profile", request.RequestUri?.AbsolutePath);
			Assert.Equal(string.Empty, request.RequestUri?.Query);
			return "{}";
		});
		using var http = new HttpClient(handler);

		Assert.True((await CreateClient(http).Accounts.GetProfile("name/with?reserved#text")).IsSuccessStatusCode);
	}

	[Fact]
	public void BeneficiaryBirthDate_UsesDateOnlyWhileKeepingExistingClrType()
	{
		var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }.AddStrikeConverters();
		var beneficiary = new Beneficiary
		{
			Name = "Test",
			Type = BeneficiaryType.Individual,
			DateOfBirth = new DateTimeOffset(1990, 1, 2, 0, 0, 0, TimeSpan.FromHours(10))
		};

		using var document = JsonDocument.Parse(JsonSerializer.Serialize(beneficiary, options));
		Assert.Equal("1990-01-02", document.RootElement.GetProperty("dateOfBirth").GetString());
		var result = JsonSerializer.Deserialize<Beneficiary>("""{"name":"Test","type":"INDIVIDUAL","dateOfBirth":"1990-01-02"}""", options);
		Assert.Equal("1990-01-02", result!.DateOfBirth?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
		Assert.Equal(TimeSpan.Zero, result.DateOfBirth?.Offset);
	}

	[Fact]
	public async Task ErrorResponse_RetainsDevelopmentDiagnostics()
	{
		using var handler = new Handler(_ => """{"data":{"code":"INVALID_DATA","status":400},"debug":{"full":"details","body":"body"}}""", HttpStatusCode.BadRequest);
		using var http = new HttpClient(handler);
		var client = CreateClient(http);
		client.ThrowOnError = false;

		var response = await client.Accounts.GetProfile();

		Assert.Equal("details", response.Error?.Debug?.Full);
		Assert.Equal("body", response.Error?.Debug?.Body);
	}

	private static StrikeClient CreateClient(HttpClient client) => new(StrikeEnvironment.Live, "offline", new Factory(client), serverUrl: new Uri("https://strike.example/")) { ThrowOnError = true };

	private sealed class Factory(HttpClient client) : IHttpClientFactory
	{
		public HttpClient CreateClient(string name) => client;
	}

	private sealed class Handler(Func<HttpRequestMessage, string> respond, HttpStatusCode status = HttpStatusCode.OK) : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
			Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(respond(request), Encoding.UTF8, "application/json") });
	}
}
