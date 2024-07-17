using Strike.Client.Invoices;
using Strike.Client.Models;
using Strike.Client.PaymentQuotes.Lightning;
using Strike.Client.PaymentQuotes.Onchain;
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
	public async Task GetLnurlDetails_ShouldWork()
	{
		var client = GetClient();

		var details = await client.PaymentQuotes.GetLnurlDetails("frank_test@dev.strike.me");

		AssertStatus(details);
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

	[SkippableFact]
	public async Task PayToOnchain_ShouldWork()
	{
		var client = GetClient();

		var btcAddress = "tb1qggmknym2lw23e9vhjlqtddr3dkzkrz7860v3nc";
		var targetAmount = new MoneyWithFee { Amount = 10m, Currency = Currency.Usd, FeePolicy = FeePolicy.Exclusive };

		var tiers = await client.PaymentQuotes.GetOnchainTiers(new OnchainTiersReq
		{
			BtcAddress = btcAddress,
			Amount = targetAmount
		});

		AssertStatus(tiers);
		Assert.NotEmpty(tiers);

		var selectedTier = tiers.Skip(1).First().Id;

		var quote = await client.PaymentQuotes.CreateOnchainQuote(new OnchainPaymentQuoteReq
		{
			BtcAddress = btcAddress,
			Amount = targetAmount,
			SourceCurrency = targetAmount.Currency,
			OnchainTierId = selectedTier
		});
		AssertStatus(quote);
		Assert.True(quote.TotalAmount.Amount < 20m);

		var payment = await client.PaymentQuotes.ExecuteQuote(quote.PaymentQuoteId);
		AssertStatus(payment);
		Assert.Equal(PaymentState.Pending, payment.State);
	}
}
