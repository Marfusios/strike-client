using Strike.Client.Models;

namespace Strike.Client.Invoices;
public class InvoiceReq : RequestBase
{
	/// <summary>
	/// Invoice correlation id. Must be a unique value. Can be used to correlate the invoice with an external entity
	/// </summary>
	/// <example>224bff37-021f-43e5-9b9c-390e3d834720</example>
	public string? CorrelationId { get; init; }

	/// <summary>
	/// Invoice description
	/// </summary>
	/// <example>Invoice for order 123</example>
	public string? Description { get; init; }

	/// <summary>
	/// Invoice amount. Only currencies which are invoiceable for the receiver's account can be used. Invoiceable currencies can be found using get account
	/// profile endpoint.
	/// </summary>
	public required Money Amount { get; init; }
}
