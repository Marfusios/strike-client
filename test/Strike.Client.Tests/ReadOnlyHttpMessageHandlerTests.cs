using System.Net;
using Strike.Client.Testing;

namespace Strike.Client.Tests;

public class ReadOnlyHttpMessageHandlerTests
{
	[Theory]
	[InlineData("POST")]
	[InlineData("PUT")]
	[InlineData("PATCH")]
	[InlineData("DELETE")]
	[InlineData("OPTIONS")]
	public async Task WriteRequest_IsBlockedBeforeReachingTransport(string method)
	{
		using var transport = new RecordingHandler();
		using var guard = new ReadOnlyHttpMessageHandler { InnerHandler = transport };
		using var client = new HttpClient(guard);
		using var request = new HttpRequestMessage(new HttpMethod(method), "https://api.strike.me/v1/payments");

		_ = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendAsync(request));

		Assert.Equal(0, transport.RequestCount);
	}

	[Theory]
	[InlineData("GET")]
	[InlineData("HEAD")]
	public async Task ReadRequest_ReachesTransport(string method)
	{
		using var transport = new RecordingHandler();
		using var guard = new ReadOnlyHttpMessageHandler { InnerHandler = transport };
		using var client = new HttpClient(guard);
		using var request = new HttpRequestMessage(new HttpMethod(method), "https://api.strike.me/v1/balances");

		using var response = await client.SendAsync(request);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(1, transport.RequestCount);
	}

	private sealed class RecordingHandler : HttpMessageHandler
	{
		public int RequestCount { get; private set; }

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			RequestCount++;
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
		}
	}
}
