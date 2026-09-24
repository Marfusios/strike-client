namespace Strike.Client.Subscriptions;

/// <summary>
/// A subscription that delivers selected event types to a webhook endpoint.
/// </summary>
public record Subscription : ResponseBase
{
	/// <summary>
	/// The subscription ID.
	/// </summary>
	public Guid Id { get; init; }

	/// <summary>
	/// The HTTPS endpoint receiving webhook requests.
	/// </summary>
	public string? WebhookUrl { get; init; }

	/// <summary>
	/// The format used for webhook payloads.
	/// </summary>
	public WebhookVersion WebhookVersion { get; init; }

	/// <summary>
	/// Whether the subscription delivers webhook requests.
	/// </summary>
	public bool Enabled { get; init; }

	/// <summary>
	/// The time the subscription was created.
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// The event types that trigger webhook delivery.
	/// </summary>
	public IReadOnlyCollection<string> EventTypes { get; init; } = [];
}
