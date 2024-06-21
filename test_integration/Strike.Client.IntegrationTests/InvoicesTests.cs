using Strike.Client.Invoices;
using Strike.Client.Models;

namespace Strike.Client.IntegrationTests;
public class InvoicesTests : TestsBase
{
	[SkippableFact]
	public async Task InvoiceFlow_ShouldWork()
	{
		var client = GetClient();

		var invoice = await client.Invoices.IssueInvoice(new InvoiceReq
		{
			Amount = new Money { Amount = 0.00001000m, Currency = Currency.Btc },
			Description = "Invoice from Strike .NET client integration tests"
		});

		AssertStatus(invoice);
		Assert.Equal(0.00001000m, invoice.Amount.Amount);
		Assert.Equal(Currency.Btc, invoice.Amount.Currency);
		Assert.Equal(InvoiceState.Unpaid, invoice.State);

		var invoiceQuote = await client.Invoices.IssueQuote(invoice.InvoiceId);
		AssertStatus(invoiceQuote);
		Assert.NotNull(invoiceQuote.LnInvoice);
		Assert.Equal(InvoiceQuoteResult.Pending, invoiceQuote.Result);

		//var foundQuote = await client.Invoices.FindQuote(invoiceQuote.QuoteId);
		//AssertStatus(foundQuote);
		//Assert.NotNull(foundQuote.LnInvoice);
		//Assert.Equal(InvoiceQuoteResult.Pending, foundQuote.Result);

		var allInvoices = await client.Invoices.GetInvoices();
		AssertStatus(allInvoices);
		Assert.NotEmpty(allInvoices.Items);
	}

	[SkippableFact]
	public async Task InvoiceFlow_ForHandle_ShouldWork()
	{
		var client = GetClient();

		var handle = "frank_test";
		var invoice = await client.Invoices.IssueInvoiceFor(handle, new InvoiceReq
		{
			Amount = new Money { Amount = 0.1m, Currency = Currency.Eur },
			Description = "Invoice from Strike .NET client integration tests"
		});

		AssertStatus(invoice);
		Assert.Equal(0.1m, invoice.Amount.Amount);
		Assert.Equal(Currency.Eur, invoice.Amount.Currency);
		Assert.Equal(InvoiceState.Unpaid, invoice.State);
		Assert.True(invoice.IssuerId != invoice.ReceiverId);

		var invoiceQuote = await client.Invoices.IssueQuote(invoice.InvoiceId);
		AssertStatus(invoiceQuote);
		Assert.NotNull(invoiceQuote.LnInvoice);
		Assert.Equal(InvoiceQuoteResult.Pending, invoiceQuote.Result);
	}
}
