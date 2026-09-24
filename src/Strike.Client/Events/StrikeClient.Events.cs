using Strike.Client.Events;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Events API endpoints.
	/// </summary>
	public EventsClient Events => new(this);

	/// <summary>
	/// Events API endpoints. Requires the partner.webhooks.manage scope.
	/// </summary>
	public record EventsClient(StrikeClient Client)
	{
		/// <summary>
		/// Find a webhook event by ID.
		/// </summary>
		public Task<StrikeEvent> FindEvent(Guid eventId) =>
			Client.Get($"/v1/events/{eventId}")
				.ParseResponse<StrikeEvent>();

		/// <summary>
		/// Get webhook events with optional OData filtering and ordering.
		/// </summary>
		/// <param name="top">The page size, up to 100. Defaults to 50.</param>
		/// <param name="skip">The number of events to skip.</param>
		/// <param name="filter">An OData filter using created, eventType, or deliverySuccess.</param>
		/// <param name="orderBy">An OData ordering expression using created.</param>
		public Task<EventsCollection> GetEvents(int top = 50, int skip = 0, string? filter = null, string? orderBy = null)
		{
			var urlParams = ConstructUrlParams(
				(nameof(top), top),
				(nameof(skip), skip),
				(nameof(filter), filter),
				("orderby", orderBy));
			return Client.Get($"/v1/events{urlParams}")
				.ParseResponse<EventsCollection>();
		}
	}
}
