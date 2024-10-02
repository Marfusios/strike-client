using Strike.Client.ReceiveRequests;
using Strike.Client.ReceiveRequests.Requests;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Receive requests API endpoints
	/// </summary>
	public ReceiveRequestsClient ReceiveRequests => new(this);

	/// <summary>
	/// Receive requests API endpoints
	/// </summary>
	public record ReceiveRequestsClient(StrikeClient Client)
	{
		/// <summary>
		/// Create a new receive request
		/// </summary>
		public Task<ReceiveRequest> Create(ReceiveRequestReq request) =>
			Client.Post($"/v1/receive-requests", request)
				.ParseResponse<ReceiveRequest>();

		/// <summary>
		/// Find payment by id 
		/// </summary>
		public Task<ReceiveRequest> FindRequest(Guid receiveRequestId) =>
			Client.Get($"/v1/receive-requests/{receiveRequestId}")
				.ParseResponse<ReceiveRequest>();

		/// <summary>
		/// Get all receive requests
		/// </summary>
		public Task<ReceiveRequestsCollection> GetRequests(
			int top = 100,
			int skip = 0,
			string?[]? bolt11Invoice = null,
			string?[]? onchainAddress = null,
			string?[]? paymentHash = null
		)
		{
			var urlParams = ConstructUrlParams(
				(nameof(top), top),
				(nameof(skip), skip),
				(nameof(bolt11Invoice), bolt11Invoice),
				(nameof(onchainAddress), onchainAddress),
				(nameof(paymentHash), paymentHash)
			);
			return Client.Get($"/v1/receive-requests{urlParams}")
				.ParseResponse<ReceiveRequestsCollection>();
		}

		/// <summary>
		/// Get all receives
		/// </summary>
		public Task<ReceivesCollection> GetReceives(
			int top = 100,
			int skip = 0,
			string?[]? bolt11Invoice = null,
			string?[]? onchainAddress = null,
			string?[]? paymentHash = null,
			Guid[]? receiveId = null,
			Guid[]? receiveRequestId = null
		)
		{
			var urlParams = ConstructUrlParams(
				(nameof(top), top),
				(nameof(skip), skip),
				(nameof(bolt11Invoice), bolt11Invoice),
				(nameof(onchainAddress), onchainAddress),
				(nameof(paymentHash), paymentHash),
				(nameof(receiveId), receiveId),
				(nameof(receiveRequestId), receiveRequestId)
			);
			return Client.Get($"/v1/receive-requests/receives{urlParams}")
				.ParseResponse<ReceivesCollection>();
		}

		/// <summary>
		/// Get all receives for the target request
		/// </summary>
		public Task<ReceivesCollection> GetReceives(
			Guid receiveRequestId,
			int top = 100,
			int skip = 0,
			string?[]? bolt11Invoice = null,
			string?[]? onchainAddress = null,
			string?[]? paymentHash = null,
			Guid[]? receiveId = null
		)
		{
			var urlParams = ConstructUrlParams(
				(nameof(top), top),
				(nameof(skip), skip),
				(nameof(bolt11Invoice), bolt11Invoice),
				(nameof(onchainAddress), onchainAddress),
				(nameof(paymentHash), paymentHash),
				(nameof(receiveId), receiveId)
			);
			return Client.Get($"/v1/receive-requests/{receiveRequestId}/receives{urlParams}")
				.ParseResponse<ReceivesCollection>();
		}
	}
}
