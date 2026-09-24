namespace Strike.Client.Events;

/// <summary>
/// A page of webhook events.
/// </summary>
public record EventsCollection : ResponseBase
{
	/// <summary>
	/// The events in this page.
	/// </summary>
	public IReadOnlyCollection<StrikeEvent> Items { get; init; } = [];

	/// <summary>
	/// The total number of matching events. Ignore this value when IsCountUnknown is true.
	/// </summary>
	public long Count { get; init; }

	/// <summary>
	/// Whether the total number of matching events is unavailable.
	/// </summary>
	public bool IsCountUnknown { get; init; }
}
