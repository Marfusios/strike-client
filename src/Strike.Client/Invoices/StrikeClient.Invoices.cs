﻿using Strike.Client.Invoices;

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
			Client.Post($"/v1/invoices/handle/{handle}", invoice)
				.ParseResponse<Invoice>();

		/// <summary>
		/// Find invoice by id
		/// </summary>
		public Task<Invoice> FindInvoice(Guid invoiceId) =>
			Client.Get($"/v1/invoices/{invoiceId}")
				.ParseResponse<Invoice>();

		/// <summary>
		/// Get all invoices
		/// </summary>
		public Task<InvoicesCollection> GetInvoices(int top = 100, int skip = 0) =>
			Client.Get($"/v1/invoices?$top={top}&$skip={skip}")
				.ParseResponse<InvoicesCollection>();

		/// <summary>
		/// Get all invoices filtered by raw OData query
		/// </summary>
		public Task<InvoicesCollection> GetInvoices(string filter, int top = 100, int skip = 0) =>
			Client.Get($"/v1/invoices?$top={top}&$skip={skip}&$filter={filter}")
				.ParseResponse<InvoicesCollection>();

		/// <summary>
		/// Issue a new quote for the target invoice
		/// </summary>
		public Task<InvoiceQuote> IssueQuote(Guid invoiceId, InvoiceQuoteReq? request = null) =>
			Client.Post($"/v1/invoices/{invoiceId}/quote{GetDescriptionParam(request)}", request)
				.ParseResponse<InvoiceQuote>();

		/// <summary>
		/// Find quote by id
		/// </summary>
		public Task<InvoiceQuote> FindQuote(Guid quoteId) =>
			Client.Get($"/v1/quotes/{quoteId}")
				.ParseResponse<InvoiceQuote>();

		private static string GetDescriptionParam(InvoiceQuoteReq? request) =>
			string.IsNullOrWhiteSpace(request?.DescriptionHash) ?
				string.Empty :
				$"?descriptionHash={request.DescriptionHash}";
	}
}
