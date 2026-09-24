using Strike.Client.PayoutOriginators;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Payout originator API endpoints.
	/// </summary>
	public PayoutOriginatorsClient PayoutOriginators => new(this);

	/// <summary>
	/// Payout originator API endpoints.
	/// </summary>
	public record PayoutOriginatorsClient(StrikeClient Client)
	{
		/// <summary>
		/// Get a page of payout originators.
		/// </summary>
		public Task<PayoutOriginatorsCollection> GetPayoutOriginators(int top = 100, int skip = 0)
		{
			var urlParams = ConstructUrlParams((nameof(top), top), (nameof(skip), skip));
			return Client.Get($"/v1/payout-originators{urlParams}").ParseResponse<PayoutOriginatorsCollection>();
		}

		/// <summary>
		/// Create an individual or company payout originator.
		/// </summary>
		public Task<PayoutOriginator> Create(PayoutOriginatorReq request) => request switch
		{
			PayoutOriginatorIndividualReq individual => Client.Post("/v1/payout-originators", individual).ParseResponse<PayoutOriginator>(),
			PayoutOriginatorCompanyReq company => Client.Post("/v1/payout-originators", company).ParseResponse<PayoutOriginator>(),
			null => throw new ArgumentNullException(nameof(request)),
			_ => throw new ArgumentException("Use an individual or company payout originator request.", nameof(request))
		};

		/// <summary>
		/// Find a payout originator by ID.
		/// </summary>
		public Task<PayoutOriginator> FindPayoutOriginator(Guid id) =>
			Client.Get($"/v1/payout-originators/{id}").ParseResponse<PayoutOriginator>();

		/// <summary>
		/// Delete a payout originator by ID.
		/// </summary>
		public Task<PayoutOriginatorDelete> DeletePayoutOriginator(Guid id) =>
			Client.Delete($"/v1/payout-originators/{id}").ParseResponse<PayoutOriginatorDelete>();
	}
}
