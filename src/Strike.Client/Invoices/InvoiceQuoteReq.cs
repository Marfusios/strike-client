namespace Strike.Client.Invoices;
public class InvoiceQuoteReq : RequestBase
{
	/// <summary>
	/// Lightning invoice description hash hex string. If provided, the invoice description will be ignored in favor of this hash
	/// </summary>
	public string? DescriptionHash { get; init; }
}
