using Strike.Client.Accounts;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Accounts API endpoints
	/// </summary>
	public AccountsClient Accounts => new(this);

	/// <summary>
	/// Accounts API endpoints
	/// </summary>
	public record AccountsClient(StrikeClient Client)
	{
		/// <summary>
		/// Fetch public account profile of authenticated user
		/// </summary>
		public Task<AccountProfile> GetProfile() =>
			Client.GetAsync("/v1/accounts/profile")
				.ParseResponseAsync<AccountProfile>();

		/// <summary>
		/// Fetch public account profile info by ID
		/// </summary>
		public Task<AccountProfile> GetProfile(Guid accountId) =>
			Client.GetAsync($"/v1/accounts/{accountId}/profile")
				.ParseResponseAsync<AccountProfile>();

		/// <summary>
		/// Fetch public account profile info by handle
		/// </summary>
		public Task<AccountProfile> GetProfile(string handle) =>
			Client.GetAsync($"/v1/accounts/handle/{handle}/profile")
				.ParseResponseAsync<AccountProfile>();
	}
}
