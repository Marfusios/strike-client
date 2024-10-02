using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using Strike.Client;
using Strike.Client.Invoices;
using Strike.Client.Models;
using Strike.Client.PaymentQuotes.Lightning;
using Strike.Client.ReceiveRequests.Requests;

using var logFactory = InitLogging();

Console.WriteLine("|=======================|");
Console.WriteLine("|     STRIKE CLIENT     |");
Console.WriteLine("|=======================|");
Console.WriteLine();

Log.Debug("====================================");
Log.Debug("              STARTING              ");
Log.Debug("====================================");

var logger = logFactory.CreateLogger<StrikeClient>();
var options = InitOptions();

var client = new StrikeClient(options, null, logger)
{
	ShowRawJson = true
};

var invoice = await client.Invoices.IssueInvoice(new InvoiceReq
{
	Amount = new Money { Amount = 2m, Currency = Currency.Eur },
	Description = "Invoice from Strike .NET client"
});
Log.Information($"Invoice: {invoice.InvoiceId} with amount: {invoice.Amount.Amount} {invoice.Amount.Currency}");

var invoiceQuote = await client.Invoices.IssueQuote(invoice.InvoiceId);

var foundQuote = await client.Invoices.FindQuote(invoiceQuote.QuoteId);

var allInvoices = await client.Invoices.GetInvoices();

var receiveRequest = await client.ReceiveRequests.Create(new ReceiveRequestReq
{
	TargetCurrency = Currency.Eur,
	Bolt11 = new Bolt11ReceiveRequestReq
	{
		Amount = new Money { Amount = 1m, Currency = Currency.Eur },
		Description = "Receive request from Strike .NET client",
		ExpiryInSeconds = 60 * 10
	},
	Onchain = new OnchainReceiveRequestReq
	{
		Amount = new Money { Amount = 1000m, Currency = Currency.Eur }
	},
	//Bolt12 = new Bolt12ReceiveRequestReq()
});

var foundRequest = await client.ReceiveRequests.FindRequest(receiveRequest.ReceiveRequestId);
var allRequests = await client.ReceiveRequests.GetRequests();
var receives = await client.ReceiveRequests.GetReceives(receiveRequest.ReceiveRequestId);
var receivesByFilter = await client.ReceiveRequests.GetReceives(paymentHash: [receiveRequest.Bolt11?.PaymentHash, receiveRequest.Bolt11?.PaymentHash, "invalid_payment_hash"]);
var receivesByParent = await client.ReceiveRequests.GetReceives(receiveRequestId: [receiveRequest.ReceiveRequestId]);

var profile = await client.Accounts.GetProfile("marfusios");
Log.Information($"Profile: {profile.Handle} and description: {profile.Description}");

var balances = await client.Balances.GetBalances();
foreach (var balance in balances)
{
	Log.Information($"Balance: {balance.Total} {balance.Currency}");
}

var rates = await client.Rates.GetRatesTicker();
foreach (var rate in rates)
{
	Log.Information($"Rate: {rate.Amount} {rate.SourceCurrency}/{rate.TargetCurrency}");
}

var quote = await client.PaymentQuotes.CreateLnurlQuote(new LnurlPaymentQuoteReq
{
	LnAddressOrUrl = "marfusios@getalby.com",
	SourceCurrency = Currency.Eur,
	Amount = new Money { Amount = 0.05m, Currency = Currency.Eur },
	Description = "Payment by .NET client"
});

Log.Information($"Quote {quote.PaymentQuoteId}");

//var executedPayment = await client.PaymentQuotes.ExecuteQuote(quote.PaymentQuoteId);
//var foundPayment = await client.Payments.FindPayment(executedPayment.PaymentId);

Log.Debug("====================================");
Log.Debug("              STOPPING              ");
Log.Debug("====================================");
await Log.CloseAndFlushAsync();

static SerilogLoggerFactory InitLogging()
{
	var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
	var logPath = Path.Combine(executingDir ?? string.Empty, "logs", "verbose.log");
	var logger = new LoggerConfiguration()
		.MinimumLevel.Verbose()
		.WriteTo.File(logPath, rollingInterval: RollingInterval.Day, formatProvider: CultureInfo.InvariantCulture)
		.WriteTo.Console(LogEventLevel.Verbose,
			theme: AnsiConsoleTheme.Literate,
			outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message} {NewLine}{Exception}",
			formatProvider: CultureInfo.InvariantCulture)
		.CreateLogger();
	Log.Logger = logger;
	return new SerilogLoggerFactory(logger);
}

static IOptions<StrikeOptions> InitOptions()
{
	var config = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false)
		.AddEnvironmentVariables()
		.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
		.Build();

	var provider = new ServiceCollection()
		.AddStrike(config)
		.BuildServiceProvider();

	return provider.GetRequiredService<IOptions<StrikeOptions>>();
}
