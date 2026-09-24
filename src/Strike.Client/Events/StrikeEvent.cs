using Strike.Client.Subscriptions;

namespace Strike.Client.Events;

/// <summary>
/// A webhook event and its delivery status.
/// </summary>
public record StrikeEvent : ResponseBase
{
	/// <summary>
	/// The event ID.
	/// </summary>
	public Guid Id { get; init; }

	/// <summary>
	/// The event type, such as invoice.updated.
	/// </summary>
	public string? EventType { get; init; }

	/// <summary>
	/// The version used to format the event payload.
	/// </summary>
	public WebhookVersion WebhookVersion { get; init; }

	/// <summary>
	/// The event-specific JSON payload. Its shape depends on the event type and webhook version.
	/// </summary>
	public JsonElement Data { get; init; }

	/// <summary>
	/// The time the event was created.
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// True when all delivery attempts succeeded, false when any are pending or unsuccessful,
	/// or null when the event has no associated delivery attempts.
	/// </summary>
	public bool? DeliverySuccess { get; init; }
}
