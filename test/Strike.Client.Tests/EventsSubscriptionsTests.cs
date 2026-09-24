using System.Net;
using System.Text;
using System.Text.Json;
using Strike.Client.Subscriptions;

namespace Strike.Client.Tests;

public class EventsSubscriptionsTests
{
	private const string EventJson = """{"id":"11111111-1111-1111-1111-111111111111","eventType":"invoice.updated","webhookVersion":"v2","data":{"entityId":"22222222-2222-2222-2222-222222222222","changes":["state"],"custom":{"value":42}},"created":"2026-09-24T10:00:00+02:00","deliverySuccess":null}""";
	private const string SubscriptionJson = """{"id":"33333333-3333-3333-3333-333333333333","webhookUrl":"https://webhook.example/strike","webhookVersion":"v2","enabled":true,"created":"2026-09-24T10:00:00+02:00","eventTypes":["invoice.updated","receive-request.receive-completed"]}""";

	[Fact]
	[Trait("Cat", "Base")]
	public async Task FindEvent_PreservesPayloadAndNullableDeliveryStatus()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/events/11111111-1111-1111-1111-111111111111", request.RequestUri?.AbsolutePath);
			return JsonResponse(EventJson);
		});
		using var httpClient = new HttpClient(handler);
		var client = CreateClient(httpClient);
		client.ShowRawJson = true;

		var response = await client.Events.FindEvent(Guid.Parse("11111111-1111-1111-1111-111111111111"));

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), response.Id);
		Assert.Equal("invoice.updated", response.EventType);
		Assert.Equal(WebhookVersion.V2, response.WebhookVersion);
		Assert.Equal(42, response.Data.GetProperty("custom").GetProperty("value").GetInt32());
		Assert.Equal("state", response.Data.GetProperty("changes")[0].GetString());
		Assert.Equal(new DateTimeOffset(2026, 9, 24, 10, 0, 0, TimeSpan.FromHours(2)), response.Created);
		Assert.Null(response.DeliverySuccess);
		Assert.Equal(EventJson, response.RawJson);
	}

	[Theory]
	[InlineData("null", JsonValueKind.Null)]
	[InlineData("[1,\"two\"]", JsonValueKind.Array)]
	[InlineData("\"payload\"", JsonValueKind.String)]
	[Trait("Cat", "Base")]
	public async Task FindEvent_PreservesNonObjectPayloads(string payload, JsonValueKind expectedKind)
	{
		using var handler = new StubHandler(_ => JsonResponse("{\"data\":" + payload + "}"));
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Events.FindEvent(Guid.Empty);

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(expectedKind, response.Data.ValueKind);
		Assert.Equal(payload, response.Data.GetRawText());
	}

	[Theory]
	[InlineData("true", true)]
	[InlineData("false", false)]
	[Trait("Cat", "Base")]
	public async Task FindEvent_ParsesDeliveryStatusAndUnknownWebhookVersion(string deliverySuccess, bool expected)
	{
		using var handler = new StubHandler(_ => JsonResponse("{\"webhookVersion\":\"v99\",\"deliverySuccess\":" + deliverySuccess + "}"));
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Events.FindEvent(Guid.Empty);

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(WebhookVersion.Undefined, response.WebhookVersion);
		Assert.Equal(expected, response.DeliverySuccess);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task GetEvents_EncodesODataAndPreservesPaginationMetadata()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/events", request.RequestUri?.AbsolutePath);
			Assert.Equal("?$top=20&$skip=40&$filter=eventType%20eq%20%27custom%26event%27%20and%20deliverySuccess%20eq%20false&$orderby=created%20desc", request.RequestUri?.Query);
			return JsonResponse("{\"items\":[" + EventJson + "],\"count\":2147483648,\"isCountUnknown\":true}");
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Events.GetEvents(20, 40, "eventType eq 'custom&event' and deliverySuccess eq false", "created desc");

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal("invoice.updated", Assert.Single(response.Items).EventType);
		Assert.Equal(2147483648L, response.Count);
		Assert.True(response.IsCountUnknown);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task GetSubscriptions_ParsesUnwrappedArray()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/subscriptions", request.RequestUri?.AbsolutePath);
			Assert.Equal(string.Empty, request.RequestUri?.Query);
			return JsonResponse("[" + SubscriptionJson + "]");
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Subscriptions.GetSubscriptions();

		Assert.True(response.IsSuccessStatusCode);
		var subscription = Assert.Single(response);
		Assert.Equal(Guid.Parse("33333333-3333-3333-3333-333333333333"), subscription.Id);
		Assert.Equal("https://webhook.example/strike", subscription.WebhookUrl);
		Assert.Equal(WebhookVersion.V2, subscription.WebhookVersion);
		Assert.True(subscription.Enabled);
		Assert.Equal(new DateTimeOffset(2026, 9, 24, 10, 0, 0, TimeSpan.FromHours(2)), subscription.Created);
		Assert.Equal(["invoice.updated", "receive-request.receive-completed"], subscription.EventTypes);
	}

	[Theory]
	[InlineData("v1", WebhookVersion.V1)]
	[InlineData("v99", WebhookVersion.Undefined)]
	[Trait("Cat", "Base")]
	public async Task FindSubscription_ParsesKnownAndUnknownWebhookVersions(string version, WebhookVersion expected)
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Get, request.Method);
			Assert.Equal("/v1/subscriptions/33333333-3333-3333-3333-333333333333", request.RequestUri?.AbsolutePath);
			return JsonResponse(SubscriptionJson.Replace("v2", version, StringComparison.Ordinal));
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Subscriptions.FindSubscription(Guid.Parse("33333333-3333-3333-3333-333333333333"));

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(expected, response.WebhookVersion);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task CreateSubscription_SerializesDocumentedRequestFields()
	{
		using var handler = new StubHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal("/v1/subscriptions", request.RequestUri?.AbsolutePath);
			Assert.False(request.Headers.Contains("idempotency-key"));
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal("https://webhook.example/strike", body.RootElement.GetProperty("webhookUrl").GetString());
			Assert.Equal("v2", body.RootElement.GetProperty("webhookVersion").GetString());
			Assert.Equal("test-secret-for-contract", body.RootElement.GetProperty("secret").GetString());
			Assert.True(body.RootElement.GetProperty("enabled").GetBoolean());
			Assert.Equal("invoice.updated", body.RootElement.GetProperty("eventTypes")[0].GetString());
			Assert.Equal(5, body.RootElement.EnumerateObject().Count());
			return JsonResponse(SubscriptionJson, HttpStatusCode.Created);
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Subscriptions.Create(new SubscriptionReq
		{
			WebhookUrl = "https://webhook.example/strike",
			WebhookVersion = WebhookVersion.V2,
			Secret = "test-secret-for-contract",
			Enabled = true,
			EventTypes = ["invoice.updated"]
		});

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		Assert.Equal(Guid.Parse("33333333-3333-3333-3333-333333333333"), response.Id);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task UpdateSubscription_EmitsFalseAndOmitsUnspecifiedFields()
	{
		using var handler = new StubHandler(async request =>
		{
			Assert.Equal(HttpMethod.Patch, request.Method);
			Assert.Equal("/v1/subscriptions/33333333-3333-3333-3333-333333333333", request.RequestUri?.AbsolutePath);
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.False(body.RootElement.GetProperty("enabled").GetBoolean());
			_ = Assert.Single(body.RootElement.EnumerateObject());
			return JsonResponse(SubscriptionJson.Replace("true", "false", StringComparison.Ordinal));
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Subscriptions.UpdateSubscription(Guid.Parse("33333333-3333-3333-3333-333333333333"), new SubscriptionUpdateReq { Enabled = false });

		Assert.True(response.IsSuccessStatusCode);
		Assert.False(response.Enabled);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task UpdateSubscription_SerializesVersionSecretUrlAndEventTypes()
	{
		using var handler = new StubHandler(async request =>
		{
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal("https://webhook.example/updated", body.RootElement.GetProperty("webhookUrl").GetString());
			Assert.Equal("v1", body.RootElement.GetProperty("webhookVersion").GetString());
			Assert.Equal("replacement-test-secret", body.RootElement.GetProperty("secret").GetString());
			Assert.Equal("custom.future-event", body.RootElement.GetProperty("eventTypes")[0].GetString());
			Assert.False(body.RootElement.TryGetProperty("enabled", out _));
			return JsonResponse(SubscriptionJson);
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Subscriptions.UpdateSubscription(Guid.Empty, new SubscriptionUpdateReq
		{
			WebhookUrl = "https://webhook.example/updated",
			WebhookVersion = WebhookVersion.V1,
			Secret = "replacement-test-secret",
			EventTypes = ["custom.future-event"]
		});

		Assert.True(response.IsSuccessStatusCode);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task DeleteSubscription_HandlesNoContent()
	{
		using var handler = new StubHandler(request =>
		{
			Assert.Equal(HttpMethod.Delete, request.Method);
			Assert.Equal("/v1/subscriptions/33333333-3333-3333-3333-333333333333", request.RequestUri?.AbsolutePath);
			Assert.Null(request.Content);
			return JsonResponse(string.Empty, HttpStatusCode.NoContent);
		});
		using var httpClient = new HttpClient(handler);

		var response = await CreateClient(httpClient).Subscriptions.DeleteSubscription(Guid.Parse("33333333-3333-3333-3333-333333333333"));

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
	}

	private static StrikeClient CreateClient(HttpClient httpClient) =>
		new(StrikeEnvironment.Live, "test-key", new StubClientFactory(httpClient), serverUrl: new Uri("https://strike.example/")) { ThrowOnError = true };

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
