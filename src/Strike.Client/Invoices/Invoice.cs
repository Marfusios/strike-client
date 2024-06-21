using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.Invoices;

/// <summary>
/// Issued invoice
/// </summary>
[DebuggerDisplay("Invoice {Amount} {Description} | {State}")]
public record Invoice : ResponseBase
{
	public Guid InvoiceId { get; init; }
	public Money Amount { get; init; } = null!;
	public InvoiceState State { get; init; }
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// Invoice correlation id
	/// </summary>
	/// <example>224bff37-021f-43e5-9b9c-390e3d834720</example>
	public string? CorrelationId { get; init; }

	/// <summary>
	/// Invoice description
	/// </summary>
	/// <example>Pizza and beer</example>
	public string? Description { get; init; }

	/// <summary>
	/// Id of the invoice issuer
	/// </summary>
	/// <example>7e7671fb-8265-4a7e-91df-23a3b34457df</example>
	public Guid IssuerId { get; init; }

	/// <summary>
	/// Id of the invoice receiver
	/// </summary>
	/// <example>028d9fe1-069f-4bc4-a1fb-bd591bf62f5b</example>
	public Guid ReceiverId { get; init; }

	/// <summary>
	/// Id of the invoice payer. Present only when invoice was created for the dedicated payer
	/// </summary>
	/// <example>14011616-a18f-47a0-bfef-0687626bec35</example>
	public Guid? PayerId { get; init; }
}
