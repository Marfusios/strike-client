using Strike.Client.Subscriptions;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Subscriptions API endpoints.
	/// </summary>
	public SubscriptionsClient Subscriptions => new(this);

	/// <summary>
	/// Subscriptions API endpoints. Requires the partner.webhooks.manage scope.
	/// </summary>
	public record SubscriptionsClient(StrikeClient Client)
	{
		/// <summary>
		/// Get all webhook subscriptions.
		/// </summary>
		public Task<ResponseCollection<Subscription>> GetSubscriptions() =>
			Client.Get("/v1/subscriptions")
				.ParseResponse<ResponseCollection<Subscription>>();

		/// <summary>
		/// Create a webhook subscription. Strike permits at most 50 subscriptions.
		/// </summary>
		public Task<Subscription> Create(SubscriptionReq request) =>
			Client.Post("/v1/subscriptions", request)
				.ParseResponse<Subscription>();

		/// <summary>
		/// Find a webhook subscription by ID.
		/// </summary>
		public Task<Subscription> FindSubscription(Guid subscriptionId) =>
			Client.Get($"/v1/subscriptions/{subscriptionId}")
				.ParseResponse<Subscription>();

		/// <summary>
		/// Update the supplied properties of a webhook subscription.
		/// </summary>
		public Task<Subscription> UpdateSubscription(Guid subscriptionId, SubscriptionUpdateReq request) =>
			Client.Patch($"/v1/subscriptions/{subscriptionId}", request)
				.ParseResponse<Subscription>();

		/// <summary>
		/// Delete a webhook subscription. A successful response has status 204 and no subscription data.
		/// </summary>
		public Task<Subscription> DeleteSubscription(Guid subscriptionId) =>
			Client.Delete($"/v1/subscriptions/{subscriptionId}")
				.ParseResponse<Subscription>();
	}
}
