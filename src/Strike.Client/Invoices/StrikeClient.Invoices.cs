using Strike.Client.Invoices;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Invoices API endpoints
	/// </summary>
	public InvoicesClient Invoices => new(this);

	public record InvoicesClient(StrikeClient Client)
	{
		/// <summary>
		/// Issue a new invoice
		/// </summary>
		public Task<Invoice> IssueInvoice(InvoiceReq invoice) =>
			Client.PostAsync("/v1/invoices", invoice)
				.ParseResponseAsync<Invoice>();

		/// <summary>
		/// Issue a new invoice for the target account
		/// </summary>
		public Task<Invoice> IssueInvoiceFor(string handle, InvoiceReq invoice) =>
			Client.PostAsync($"/v1/invoices/handle/{handle}", invoice)
				.ParseResponseAsync<Invoice>();

		/// <summary>
		/// Find invoice by id
		/// </summary>
		public Task<Invoice> FindInvoice(Guid invoiceId) =>
			Client.GetAsync($"/v1/invoices/{invoiceId}")
				.ParseResponseAsync<Invoice>();

		/// <summary>
		/// Get all invoices
		/// </summary>
		public Task<InvoicesCollection> GetInvoices(int top = 100, int skip = 0) =>
			Client.GetAsync($"/v1/invoices?$top={top}&$skip={skip}")
				.ParseResponseAsync<InvoicesCollection>();

		/// <summary>
		/// Issue a new quote for the target invoice
		/// </summary>
		public Task<InvoiceQuote> IssueQuote(Guid invoiceId, InvoiceQuoteReq? request = null) =>
			Client.PostAsync($"/v1/invoices/{invoiceId}/quote", request)
				.ParseResponseAsync<InvoiceQuote>();
	}
}
