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
			Client.Post("/v1/invoices", invoice)
				.ParseResponse<Invoice>();

		/// <summary>
		/// Issue a new invoice for the target account
		/// </summary>
		public Task<Invoice> IssueInvoiceFor(string handle, InvoiceReq invoice) =>
			Client.Post($"/v1/invoices/handle/{Uri.EscapeDataString(handle)}", invoice)
				.ParseResponse<Invoice>();

		/// <summary>
		/// Find invoice by id
		/// </summary>
		public Task<Invoice> FindInvoice(Guid invoiceId) =>
			Client.Get($"/v1/invoices/{invoiceId}")
				.ParseResponse<Invoice>();

		/// <summary>
		/// Find an invoice and optionally include its transactions.
		/// </summary>
		public Task<Invoice> FindInvoice(Guid invoiceId, bool includeTransactions) =>
			Client.Get($"/v1/invoices/{invoiceId}?includeTransactions={(includeTransactions ? "true" : "false")}")
				.ParseResponse<Invoice>();

		/// <summary>
		/// Cancel an unpaid invoice.
		/// </summary>
		public Task<Invoice> CancelInvoice(Guid invoiceId) =>
			Client.Patch($"/v1/invoices/{invoiceId}/cancel")
				.ParseResponse<Invoice>();

		/// <summary>
		/// Get all invoices
		/// </summary>
		public Task<InvoicesCollection> GetInvoices(int top = 100, int skip = 0) =>
			GetInvoices(null, top, skip, null);

		/// <summary>
		/// Get all invoices filtered by raw OData query
		/// </summary>
		public Task<InvoicesCollection> GetInvoices(string filter, int top = 100, int skip = 0) =>
			GetInvoices(filter, top, skip, null);

		/// <summary>
		/// Get invoices with optional OData filtering and ordering, for example "created desc".
		/// </summary>
		public Task<InvoicesCollection> GetInvoices(string? filter, int top, int skip, string? orderBy)
		{
			var parameters = ConstructUrlParams((nameof(top), top), (nameof(skip), skip),
				(nameof(filter), filter), ("orderby", orderBy));
			return Client.Get($"/v1/invoices{parameters}").ParseResponse<InvoicesCollection>();
		}

		/// <summary>
		/// Issue a new quote for the target invoice
		/// </summary>
		public Task<InvoiceQuote> IssueQuote(Guid invoiceId, InvoiceQuoteReq? request = null) =>
			Client.Post($"/v1/invoices/{invoiceId}/quote", request)
				.ParseResponse<InvoiceQuote>();

		/// <summary>
		/// Find quote by id
		/// </summary>
		/// <remarks>This legacy endpoint is retained for compatibility but is absent from the current Strike API reference.</remarks>
		public Task<InvoiceQuote> FindQuote(Guid quoteId) =>
			Client.Get($"/v1/quotes/{quoteId}")
				.ParseResponse<InvoiceQuote>();
	}
}
