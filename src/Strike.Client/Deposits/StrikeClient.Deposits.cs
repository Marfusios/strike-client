using Strike.Client.Deposits;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Payments API endpoints
	/// </summary>
	public DepositsClient Deposits => new(this);

	/// <summary>
	/// Payments API endpoints
	/// </summary>
	public record DepositsClient(StrikeClient Client)
	{
		/// <summary>
		/// Initiate deposit
		/// </summary>
		public Task<Deposit> Create(DepositReq request) =>
			Client.Post($"/v1/deposits", request)
				.ParseResponse<Deposit>();

		/// <summary>
		/// Get all deposits
		/// </summary>
		public Task<DepositsCollection> GetDeposits(
			int top = 100,
			int skip = 0
			)
		{
			var urlParams = ConstructUrlParams(
				(nameof(top), top),
				(nameof(skip), skip)
			);
			return Client.Get($"/v1/deposits{urlParams}")
				.ParseResponse<DepositsCollection>();
		}

		/// <summary>
		/// Get deposit 
		/// </summary>
		public Task<Deposit> FindDeposit(Guid id) =>
			Client.Get($"/v1/deposits/{id}")
				.ParseResponse<Deposit>();

		/// <summary>
		/// Get deposit fee estimate
		/// </summary>
		public Task<DepositFeeEstimate> GetDepositFeeEstimate(DepositReq request) =>
			Client.Post($"/v1/deposits/fee", request)
				.ParseResponse<DepositFeeEstimate>();
	}
}
