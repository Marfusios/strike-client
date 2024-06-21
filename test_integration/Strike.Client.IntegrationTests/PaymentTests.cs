using Strike.Client.Invoices;
using Strike.Client.Models;
using Strike.Client.PaymentQuotes;
using Strike.Client.Payments;

namespace Strike.Client.IntegrationTests;
public class PaymentTests : TestsBase
{
	[SkippableFact]
	public async Task PayToLnurl_ShouldWork()
	{
		var client = GetClient();

		var quote = await client.PaymentQuotes.CreateLnurlQuote(new LnurlPaymentQuoteReq
		{
			LnAddressOrUrl = "frank_test@dev.strike.me",
			SourceCurrency = Currency.Usd,
			Amount = new Money { Amount = 0.05m, Currency = Currency.Usd },
			Description = "Payment by .NET client integration tests"
		});

		AssertStatus(quote);
		Assert.True(quote.TotalAmount.Amount < 1m);

		var payment = await client.PaymentQuotes.ExecuteQuote(quote.PaymentQuoteId);
		AssertStatus(payment);
		Assert.Equal(PaymentState.Completed, payment.State);
	}

	[SkippableFact]
	public async Task PayToLn_ShouldWork()
	{
		var client = GetClient();

		var handle = "frank_test";
		var invoice = await client.Invoices.IssueInvoiceFor(handle, new InvoiceReq
		{
			Amount = new Money { Amount = 0.06m, Currency = Currency.Eur },
			Description = "Invoice from Strike .NET client integration tests"
		});
		AssertStatus(invoice);

		var invoiceQuote = await client.Invoices.IssueQuote(invoice.InvoiceId);
		AssertStatus(invoiceQuote);

		var paymentQuote = await client.PaymentQuotes.CreateLnQuote(new LnPaymentQuoteReq
		{
			LnInvoice = invoiceQuote.LnInvoice,
			SourceCurrency = Currency.Usd
		});
		AssertStatus(paymentQuote);
		Assert.True(paymentQuote.TotalAmount.Amount < 1m);

		var payment = await client.PaymentQuotes.ExecuteQuote(paymentQuote.PaymentQuoteId);
		AssertStatus(payment);
		Assert.Equal(PaymentState.Completed, payment.State);
	}
}
