using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using Strike.Client;
using Strike.Client.Models;
using Strike.Client.PaymentQuotes;
using Environment = Strike.Client.Environment;

var apiKey = "YOUR_API_KEY";
var environment = Environment.Live;

using var logFactory = InitLogging();

Console.WriteLine("|=======================|");
Console.WriteLine("|     STRIKE CLIENT     |");
Console.WriteLine("|=======================|");
Console.WriteLine();

Log.Debug("====================================");
Log.Debug("              STARTING              ");
Log.Debug("====================================");

var logger = logFactory.CreateLogger<StrikeClient>();

var client = new StrikeClient(environment, apiKey, null, logger)
{
	ShowRawJson = true
};

var profile = await client.Accounts.GetProfile("marfusios");
Log.Information($"Profile: {profile.Handle} and description: {profile.Description}");

var quote = await client.PaymentQuotes.CreateLnurlQuote(new CreateLnurlPaymentQuoteReq
{
	LnAddressOrUrl = "marfusios@primal.net",
	SourceCurrency = Currency.Eur,
	Amount = new Money { Amount = 1m, Currency = Currency.Eur },
	// Description = "Test desc"
});

Log.Information($"Quote {quote.PaymentQuoteId}");

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
