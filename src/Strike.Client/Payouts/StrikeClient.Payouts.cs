using Strike.Client.Payouts;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Payout API endpoints.
	/// </summary>
	public PayoutsClient Payouts => new(this);

	/// <summary>
	/// Payout API endpoints.
	/// </summary>
	public record PayoutsClient(StrikeClient Client)
	{
		/// <summary>
		/// Get a page of payouts.
		/// </summary>
		public Task<PayoutsCollection> GetPayouts(int top = 100, int skip = 0)
		{
			var urlParams = ConstructUrlParams((nameof(top), top), (nameof(skip), skip));
			return Client.Get($"/v1/payouts{urlParams}").ParseResponse<PayoutsCollection>();
		}

		/// <summary>
		/// Create a payout for a bank payment method.
		/// </summary>
		public Task<Payout> Create(PayoutReq request) =>
			Client.Post("/v1/payouts", request).ParseResponse<Payout>();

		/// <summary>
		/// Find a payout by ID.
		/// </summary>
		public Task<Payout> FindPayout(Guid id) =>
			Client.Get($"/v1/payouts/{id}").ParseResponse<Payout>();

		/// <summary>
		/// Initiate a previously created payout.
		/// </summary>
		public Task<Payout> InitiatePayout(Guid id) =>
			Client.Patch($"/v1/payouts/{id}/initiate").ParseResponse<Payout>();
	}
}
