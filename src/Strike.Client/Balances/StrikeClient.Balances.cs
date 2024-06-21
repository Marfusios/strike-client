using Strike.Client.Balances;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Balances API endpoints
	/// </summary>
	public BalancesClient Balances => new(this);

	/// <summary>
	/// Balances API endpoints
	/// </summary>
	public record BalancesClient(StrikeClient Client)
	{
		/// <summary>
		/// Get account balance details
		/// </summary>
		public Task<ResponseCollection<Balance>> GetBalances() =>
			Client.Get("/v1/balances")
				.ParseResponseAsync<ResponseCollection<Balance>>();
	}
}
