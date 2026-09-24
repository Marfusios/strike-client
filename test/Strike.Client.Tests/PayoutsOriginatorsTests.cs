using System.Net;
using System.Text;
using System.Text.Json;
using Strike.Client.Models;
using Strike.Client.PayoutOriginators;
using Strike.Client.Payouts;

namespace Strike.Client.Tests;

public class PayoutsOriginatorsTests
{
	private const string PayoutJson = """
		{"id":"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa","state":"COMPLETED","created":"2026-09-01T12:30:00Z",
		 "paymentMethodId":"bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb","originatorId":"cccccccc-cccc-cccc-cccc-cccccccccccc",
		 "amount":{"amount":"100.25","currency":"USD"},"fee":{"amount":"0.25","currency":"USD","feePolicy":"EXCLUSIVE"},
		 "reference":"Invoice 42","initiated":"2026-09-01T12:31:00Z","completed":"2026-09-01T12:32:00Z",
		 "transactionId":"dddddddd-dddd-dddd-dddd-dddddddddddd"}
		""";
	private const string OriginatorJson = """
		{"id":"cccccccc-cccc-cccc-cccc-cccccccccccc","state":"APPROVED","created":"2026-09-01T12:30:00Z",
		 "details":{"type":"INDIVIDUAL","name":"Jane Example","dateOfBirth":"1990-02-03",
		 "address":{"country":"US","state":"NY","city":"New York","postCode":"10001","line1":"1 Example Street"}}}
		""";

	[Fact]
	public async Task GetPayouts_SendsPaginationAndReadsFullPage()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("https://strike.example/v1/payouts?$top=5&$skip=10", request.RequestUri?.AbsoluteUri);
			return JsonResponse("{\"items\":[" + PayoutJson + "],\"count\":3000000000,\"isCountUnknown\":true}");
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.Payouts.GetPayouts(top: 5, skip: 10);

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(3000000000L, response.Count);
		Assert.True(response.IsCountUnknown);
		AssertFullPayout(Assert.Single(response.Items));
	}

	[Fact]
	public async Task CreatePayout_SendsDocumentedFieldsAndReadsCreatedPayout()
	{
		using var handler = new StubHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/v1/payouts", request.RequestUri?.AbsolutePath);
			using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			var body = json.RootElement;
			Assert.Equal("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", body.GetProperty("paymentMethodId").GetString());
			Assert.Equal("cccccccc-cccc-cccc-cccc-cccccccccccc", body.GetProperty("originatorId").GetString());
			Assert.Equal("100.25", body.GetProperty("amount").GetString());
			Assert.Equal("Invoice 42", body.GetProperty("reference").GetString());
			Assert.Equal("EXCLUSIVE", body.GetProperty("feePolicy").GetString());
			Assert.Equal(5, body.EnumerateObject().Count());
			return JsonResponse(PayoutJson, HttpStatusCode.Created);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.Payouts.Create(new PayoutReq
		{
			PaymentMethodId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
			OriginatorId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
			Amount = "100.25",
			Reference = "Invoice 42",
			FeePolicy = FeePolicy.Exclusive
		});

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		AssertFullPayout(response);
	}

	[Fact]
	public async Task CreatePayout_OmitsOptionalRequestFieldsAndPreservesNullableResponseFields()
	{
		using var handler = new StubHandler(async request =>
		{
			using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal(2, json.RootElement.EnumerateObject().Count());
			Assert.Equal("1.00", json.RootElement.GetProperty("amount").GetString());
			return JsonResponse("""
				{"id":"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa","state":"NEW","created":"2026-09-01T12:30:00Z",
				 "paymentMethodId":"bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb","amount":{"amount":"1.00","currency":"EUR"}}
				""", HttpStatusCode.Created);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.Payouts.Create(new PayoutReq
		{
			PaymentMethodId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
			Amount = "1.00"
		});

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(PayoutState.New, response.State);
		Assert.Equal(Currency.Eur, response.Amount.Currency);
		Assert.Null(response.OriginatorId);
		Assert.Null(response.Fee);
		Assert.Null(response.Reference);
		Assert.Null(response.Initiated);
		Assert.Null(response.Completed);
		Assert.Null(response.TransactionId);
	}

	[Theory]
	[InlineData("NEW", PayoutState.New)]
	[InlineData("INITIATED", PayoutState.Initiated)]
	[InlineData("COMPLETED", PayoutState.Completed)]
	[InlineData("FAILED", PayoutState.Failed)]
	[InlineData("REVERSED", PayoutState.Reversed)]
	[InlineData("FUTURE_STATE", PayoutState.Undefined)]
	public async Task FindPayout_UsesIdRouteAndParsesState(string state, PayoutState expectedState)
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/payouts/aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", request.RequestUri?.AbsolutePath);
			return JsonResponse(PayoutJson.Replace("COMPLETED", state, StringComparison.Ordinal));
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.Payouts.FindPayout(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(expectedState, response.State);
	}

	[Fact]
	public async Task InitiatePayout_SendsBodylessPatchAndReadsPayout()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Patch, request.Method);
			Assert.Equal("/v1/payouts/aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa/initiate", request.RequestUri?.AbsolutePath);
			Assert.Null(request.Content);
			return JsonResponse(PayoutJson);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.Payouts.InitiatePayout(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

		AssertFullPayout(response);
	}

	[Fact]
	public async Task GetPayoutOriginators_SendsPaginationAndReadsFullPage()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("https://strike.example/v1/payout-originators?$top=7&$skip=14", request.RequestUri?.AbsoluteUri);
			return JsonResponse("{\"items\":[" + OriginatorJson + "],\"count\":3000000000,\"isCountUnknown\":true}");
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.PayoutOriginators.GetPayoutOriginators(top: 7, skip: 14);

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(3000000000L, response.Count);
		Assert.True(response.IsCountUnknown);
		var originator = Assert.Single(response.Items);
		Assert.Equal(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), originator.Id);
		Assert.Equal(PayoutOriginatorState.Approved, originator.State);
		Assert.Equal(new DateTimeOffset(2026, 9, 1, 12, 30, 0, TimeSpan.Zero), originator.Created);
		Assert.Equal(BeneficiaryType.Individual, originator.Details.Type);
		Assert.Equal("Jane Example", originator.Details.Name);
		Assert.Equal(new DateOnly(1990, 2, 3), originator.Details.DateOfBirth);
		Assert.Equal("NY", originator.Details.Address.State);
	}

	[Fact]
	public async Task CreateIndividualOriginator_PreservesDerivedFieldsThroughBaseRequest()
	{
		using var handler = new StubHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/v1/payout-originators", request.RequestUri?.AbsolutePath);
			Assert.Equal("example-value", Assert.Single(request.Headers.GetValues("X-Example")));
			using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			var body = json.RootElement;
			Assert.Equal("INDIVIDUAL", body.GetProperty("type").GetString());
			Assert.Equal("Jane Example", body.GetProperty("name").GetString());
			Assert.Equal("1990-02-03", body.GetProperty("dateOfBirth").GetString());
			Assert.Equal("US", body.GetProperty("address").GetProperty("country").GetString());
			Assert.Equal(4, body.EnumerateObject().Count());
			return JsonResponse(OriginatorJson, HttpStatusCode.Created);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		PayoutOriginatorReq originator = new PayoutOriginatorIndividualReq
		{
			Name = "Jane Example",
			Address = ExampleAddress(),
			DateOfBirth = new DateOnly(1990, 2, 3),
			AdditionalHeaders = new() { ["X-Example"] = "example-value" },
			ShowRawJson = true
		};

		var response = await client.PayoutOriginators.Create(originator);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		Assert.Equal(OriginatorJson, response.RawJson);
		Assert.Equal(BeneficiaryType.Individual, response.Details.Type);
	}

	[Fact]
	public async Task CreateCompanyOriginator_PreservesCompanyContactFieldsThroughBaseRequest()
	{
		const string CompanyJson = """
			{"id":"cccccccc-cccc-cccc-cccc-cccccccccccc","state":"PENDING_REVIEW","created":"2026-09-01T12:30:00Z",
			 "details":{"type":"COMPANY","name":"Example Company","email":"company@example.test","phoneNumber":"+15551234567",
			 "url":"https://example.test/","address":{"country":"US","city":"New York","postCode":"10001","line1":"1 Example Street"}}}
			""";
		using var handler = new StubHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/v1/payout-originators", request.RequestUri?.AbsolutePath);
			using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			var body = json.RootElement;
			Assert.Equal("COMPANY", body.GetProperty("type").GetString());
			Assert.Equal("Example Company", body.GetProperty("name").GetString());
			Assert.Equal("company@example.test", body.GetProperty("email").GetString());
			Assert.Equal("+15551234567", body.GetProperty("phoneNumber").GetString());
			Assert.Equal("https://example.test/", body.GetProperty("url").GetString());
			Assert.Equal(6, body.EnumerateObject().Count());
			return JsonResponse(CompanyJson, HttpStatusCode.Created);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		PayoutOriginatorReq originator = new PayoutOriginatorCompanyReq
		{
			Name = "Example Company",
			Address = ExampleAddress(),
			Email = "company@example.test",
			PhoneNumber = "+15551234567",
			Url = "https://example.test/"
		};

		var response = await client.PayoutOriginators.Create(originator);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		Assert.Equal(BeneficiaryType.Company, response.Details.Type);
		Assert.Equal("company@example.test", response.Details.Email);
		Assert.Equal("+15551234567", response.Details.PhoneNumber);
		Assert.Equal("https://example.test/", response.Details.Url);
		Assert.Null(response.Details.DateOfBirth);
		Assert.Null(response.Details.Address.State);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public async Task CreateOriginator_OmitsOptionalFields(bool company)
	{
		using var handler = new StubHandler(async request =>
		{
			using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal(3, json.RootElement.EnumerateObject().Count());
			Assert.Equal(company ? "COMPANY" : "INDIVIDUAL", json.RootElement.GetProperty("type").GetString());
			return JsonResponse(OriginatorJson, HttpStatusCode.Created);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		PayoutOriginatorReq request = company
			? new PayoutOriginatorCompanyReq { Name = "Example Company", Address = ExampleAddress() }
			: new PayoutOriginatorIndividualReq { Name = "Jane Example", Address = ExampleAddress() };

		var response = await client.PayoutOriginators.Create(request);

		Assert.True(response.IsSuccessStatusCode);
	}

	[Theory]
	[InlineData("PENDING_REVIEW", PayoutOriginatorState.PendingReview)]
	[InlineData("APPROVED", PayoutOriginatorState.Approved)]
	[InlineData("REJECTED", PayoutOriginatorState.Rejected)]
	[InlineData("INACTIVE", PayoutOriginatorState.Inactive)]
	[InlineData("FUTURE_STATE", PayoutOriginatorState.Undefined)]
	public async Task FindPayoutOriginator_UsesIdRouteAndParsesState(string state, PayoutOriginatorState expectedState)
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/payout-originators/cccccccc-cccc-cccc-cccc-cccccccccccc", request.RequestUri?.AbsolutePath);
			return JsonResponse(OriginatorJson.Replace("APPROVED", state, StringComparison.Ordinal)
				.Replace("INDIVIDUAL", "FUTURE_TYPE", StringComparison.Ordinal));
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.PayoutOriginators.FindPayoutOriginator(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(expectedState, response.State);
		Assert.Equal(BeneficiaryType.Undefined, response.Details.Type);
	}

	[Fact]
	public async Task DeletePayoutOriginator_SendsDeleteAndAcceptsNoContent()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Delete, request.Method);
			Assert.Equal("/v1/payout-originators/cccccccc-cccc-cccc-cccc-cccccccccccc", request.RequestUri?.AbsolutePath);
			Assert.Null(request.Content);
			return new HttpResponseMessage(HttpStatusCode.NoContent);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);

		var response = await client.PayoutOriginators.DeletePayoutOriginator(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
	}

	private static void AssertFullPayout(Payout payout)
	{
		Assert.Equal(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), payout.Id);
		Assert.Equal(PayoutState.Completed, payout.State);
		Assert.Equal(new DateTimeOffset(2026, 9, 1, 12, 30, 0, TimeSpan.Zero), payout.Created);
		Assert.Equal(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), payout.PaymentMethodId);
		Assert.Equal(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), payout.OriginatorId);
		Assert.Equal(100.25m, payout.Amount.Amount);
		Assert.Equal(Currency.Usd, payout.Amount.Currency);
		Assert.Equal(0.25m, payout.Fee?.Amount);
		Assert.Equal(Currency.Usd, payout.Fee?.Currency);
		Assert.Equal(FeePolicy.Exclusive, payout.Fee?.FeePolicy);
		Assert.Equal("Invoice 42", payout.Reference);
		Assert.Equal(new DateTimeOffset(2026, 9, 1, 12, 31, 0, TimeSpan.Zero), payout.Initiated);
		Assert.Equal(new DateTimeOffset(2026, 9, 1, 12, 32, 0, TimeSpan.Zero), payout.Completed);
		Assert.Equal(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), payout.TransactionId);
	}

	private static Address ExampleAddress() => new()
	{
		Country = "US",
		State = "NY",
		City = "New York",
		PostCode = "10001",
		Line1 = "1 Example Street"
	};

	private static StrikeClient CreateClient(HttpClient httpClient) =>
		new(StrikeEnvironment.Live, "test-key", new StubClientFactory(httpClient), serverUrl: new Uri("https://strike.example/"))
		{ ThrowOnError = true };

	private static HttpResponseMessage JsonResponse(string json, HttpStatusCode status = HttpStatusCode.OK) =>
		new(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

	private sealed class StubClientFactory(HttpClient client) : IHttpClientFactory
	{
		public HttpClient CreateClient(string name) => client;
	}

	private sealed class StubHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> response) : HttpMessageHandler
	{
		public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> response)
			: this(request => Task.FromResult(response(request)))
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => response(request);
	}
}
