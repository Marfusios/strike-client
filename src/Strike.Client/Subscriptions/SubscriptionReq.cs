namespace Strike.Client.Subscriptions;

/// <summary>
/// A request to create a webhook subscription.
/// </summary>
public class SubscriptionReq : RequestBase
{
	/// <summary>
	/// The HTTPS endpoint receiving webhook requests.
	/// </summary>
	public required string WebhookUrl { get; init; }

	/// <summary>
	/// The format to use for webhook payloads.
	/// </summary>
	public required WebhookVersion WebhookVersion { get; init; }

	/// <summary>
	/// The secret used to sign webhook requests. Must contain 10 to 50 characters.
	/// </summary>
	public required string Secret { get; init; }

	/// <summary>
	/// Whether to enable webhook delivery.
	/// </summary>
	public required bool Enabled { get; init; }

	/// <summary>
	/// One or more event types that trigger webhook delivery.
	/// </summary>
	public required IReadOnlyCollection<string> EventTypes { get; init; }
}
