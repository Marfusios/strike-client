namespace Strike.Client.Subscriptions;

/// <summary>
/// Changes to a webhook subscription. Unspecified properties are omitted from the request.
/// </summary>
public class SubscriptionUpdateReq : RequestBase
{
	/// <summary>
	/// The new HTTPS endpoint receiving webhook requests.
	/// </summary>
	public string? WebhookUrl { get; init; }

	/// <summary>
	/// The new format to use for webhook payloads.
	/// </summary>
	public WebhookVersion? WebhookVersion { get; init; }

	/// <summary>
	/// The new secret used to sign webhook requests. Must contain 10 to 50 characters.
	/// </summary>
	public string? Secret { get; init; }

	/// <summary>
	/// Whether to enable webhook delivery. Null leaves the current setting unchanged.
	/// </summary>
	public bool? Enabled { get; init; }

	/// <summary>
	/// The replacement event types that trigger webhook delivery. When specified, provide at least one.
	/// </summary>
	public IReadOnlyCollection<string>? EventTypes { get; init; }
}
