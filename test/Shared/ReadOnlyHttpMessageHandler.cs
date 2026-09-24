namespace Strike.Client.Testing;

internal sealed class ReadOnlyHttpMessageHandler : DelegatingHandler
{
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request.Method != HttpMethod.Get && request.Method != HttpMethod.Head)
			throw new InvalidOperationException("Integration tests allow only GET and HEAD requests. Write requests are blocked.");

		return base.SendAsync(request, cancellationToken);
	}
}
