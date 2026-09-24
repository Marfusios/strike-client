using System.Net;
using System.Text;
using System.Text.Json;
using Strike.Client.Deposits;
using Strike.Client.CurrencyExchanges;
using Strike.Client.Invoices;
using Strike.Client.Models;
using Strike.Client.PaymentMethods;
using Strike.Client.PaymentQuotes;
using Strike.Client.PaymentQuotes.Lightning;
using Strike.Client.PaymentQuotes.Onchain;
using Strike.Client.Payments;
using Strike.Client.ReceiveRequests.Requests;

namespace Strike.Client.Tests;

public class ExistingApiContractTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	[Trait("Cat", "Base")]
	public async Task DepositRequest_LegacyFeePropertyUsesDocumentedFeePolicy(bool estimate)
	{
		using var handler = new ContractHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal(estimate ? "/v1/deposits/fee" : "/v1/deposits", request.RequestUri!.AbsolutePath);
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal("INCLUSIVE", body.RootElement.GetProperty("feePolicy").GetString());
			Assert.DoesNotContain(body.RootElement.EnumerateObject(), p => p.Name.Equals("fee", StringComparison.OrdinalIgnoreCase));
			return JsonResponse("{}");
		});
		using var http = new HttpClient(handler);
		var client = CreateClient(http);
		var deposit = new DepositReq { PaymentMethodId = Guid.NewGuid(), Amount = "10.00", Fee = FeePolicy.Inclusive };

		if (estimate)
			_ = await client.Deposits.GetDepositFeeEstimate(deposit);
		else
			_ = await client.Deposits.Create(deposit);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task IssueQuote_SendsDescriptionHashOnlyInBody()
	{
		var id = Guid.NewGuid();
		using var handler = new ContractHandler(async request =>
		{
			Assert.Equal(HttpMethod.Post, request.Method);
			Assert.Equal($"/v1/invoices/{id}/quote", request.RequestUri!.AbsolutePath);
			Assert.Empty(request.RequestUri.Query);
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal("hash", body.RootElement.EnumerateObject().Single().Value.GetString());
			return JsonResponse("{}");
		});
		using var http = new HttpClient(handler);

		_ = await CreateClient(http).Invoices.IssueQuote(id, new InvoiceQuoteReq { DescriptionHash = "hash" });
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task GetLnurlDetails_EncodesUrlAsSinglePathSegment()
	{
		const string Lnurl = "https://receiver.example/pay/alice?amount=2&note=a#memo";
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal("/v1/payment-quotes/lightning/lnurl/" + Uri.EscapeDataString(Lnurl), request.RequestUri!.AbsolutePath);
			Assert.Empty(request.RequestUri.Query);
			Assert.Empty(request.RequestUri.Fragment);
			return Task.FromResult(JsonResponse("{}"));
		});
		using var http = new HttpClient(handler);

		_ = await CreateClient(http).PaymentQuotes.GetLnurlDetails(Lnurl);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task GetInvoices_EncodesODataFilterAsSingleValue()
	{
		const string Filter = "correlationId eq 'pizza & beer#1'";
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal("?$top=100&$skip=0&$filter=" + Uri.EscapeDataString(Filter), request.RequestUri!.Query);
			Assert.Empty(request.RequestUri.Fragment);
			return Task.FromResult(JsonResponse("{}"));
		});
		using var http = new HttpClient(handler);

		_ = await CreateClient(http).Invoices.GetInvoices(Filter);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task DepositEstimate_ImmediateSettlementAcceptsNullTimestamp()
	{
		using var handler = RespondWith("""{"settledAt":null,"settlementPeriodInDay":null}""");
		using var http = new HttpClient(handler);

		var result = await CreateClient(http).Deposits.GetDepositFeeEstimate(new DepositReq { PaymentMethodId = Guid.NewGuid(), Amount = "10" });

		Assert.True(result.IsSuccessStatusCode);
		Assert.Null(result.SettlementTime);
		Assert.Equal(default, result.SettledAt);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task ReceiveRequest_ZeroAmountInvoiceAcceptsNullBtcAmounts()
	{
		using var handler = RespondWith("""{"bolt11":{"invoice":"lnbc","paymentHash":"hash","btcAmount":null},"onchain":{"address":"bc1","btcAmount":null}}""");
		using var http = new HttpClient(handler);

		var result = await CreateClient(http).ReceiveRequests.FindRequest(Guid.NewGuid());

		Assert.True(result.IsSuccessStatusCode);
		Assert.Null(result.Bolt11!.RequestedBtcAmount);
		Assert.Null(result.Onchain!.RequestedBtcAmount);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task BankPaymentMethod_NonAchAccountAcceptsNullAccountType()
	{
		using var handler = RespondWith("""{"transferType":"SEPA","accountType":null,"currency":"EUR"}""");
		using var http = new HttpClient(handler);

		var result = await CreateClient(http).PaymentMethods.FindPaymentMethod(Guid.NewGuid());

		Assert.True(result.IsSuccessStatusCode);
		Assert.Null(result.OptionalAccountType);
		Assert.Equal(Currency.Eur, result.Currency);
	}

	[Theory]
	[InlineData("invoices")]
	[InlineData("deposits")]
	[InlineData("payment-methods")]
	[InlineData("requests")]
	[InlineData("receives")]
	[Trait("Cat", "Base")]
	public async Task Collections_AcceptDocumentedInt64Count(string resource)
	{
		using var handler = RespondWith("""{"items":[],"count":2147483648,"isCountUnknown":true}""");
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		ResponseBase response = resource switch
		{
			"invoices" => await client.Invoices.GetInvoices(),
			"deposits" => await client.Deposits.GetDeposits(),
			"payment-methods" => await client.PaymentMethods.GetPaymentMethods(),
			"requests" => await client.ReceiveRequests.GetRequests(),
			_ => await client.ReceiveRequests.GetReceives()
		};

		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(2147483648L, response.GetType().GetProperty("TotalCount")!.GetValue(response));
		Assert.Equal(true, response.GetType().GetProperty("IsCountUnknown")!.GetValue(response));
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task Balances_ExposesTradingPendingAndReservedAmounts()
	{
		using var handler = RespondWith("""[{"currency":"AUD","current":"120.50","pending":"20","reserved":"5","available":"95.50","outgoing":"2","total":"97.50"}]""");
		using var http = new HttpClient(handler);

		var balance = Assert.Single(await CreateClient(http).Balances.GetBalances());

		Assert.Equal(Currency.Aud, balance.Currency);
		Assert.Equal(120.50m, balance.Current);
		Assert.Equal(20m, balance.Pending);
		Assert.Equal(5m, balance.Reserved);
		Assert.Equal(2m, balance.Outgoing);
		Assert.Equal(97.50m, balance.Total);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task CancelInvoice_UsesPatchAndReturnsCancelledInvoice()
	{
		var id = Guid.NewGuid();
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal(HttpMethod.Patch, request.Method);
			Assert.Equal($"/v1/invoices/{id}/cancel", request.RequestUri!.AbsolutePath);
			Assert.Null(request.Content);
			return Task.FromResult(JsonResponse($$"""{"invoiceId":"{{id}}","state":"CANCELLED"}"""));
		});
		using var http = new HttpClient(handler);

		var invoice = await CreateClient(http).Invoices.CancelInvoice(id);

		Assert.Equal(id, invoice.InvoiceId);
		Assert.Equal(InvoiceState.Canceled, invoice.State);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	[Trait("Cat", "Base")]
	public async Task FindInvoice_CanRequestTransactions(bool includeTransactions)
	{
		var transactionId = Guid.NewGuid();
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal("?includeTransactions=" + (includeTransactions ? "true" : "false"), request.RequestUri!.Query);
			return Task.FromResult(JsonResponse($$"""{"transactions":[{"transactionId":"{{transactionId}}","state":"COMPLETED","amountReceived":{"amount":"12","currency":"USD"},"created":"2026-01-01T00:00:00Z","completed":"2026-01-01T00:01:00Z"}]}"""));
		});
		using var http = new HttpClient(handler);

		var invoice = await CreateClient(http).Invoices.FindInvoice(Guid.NewGuid(), includeTransactions);

		var transaction = Assert.Single(invoice.Transactions!);
		Assert.Equal(transactionId, transaction.TransactionId);
		Assert.Equal(InvoiceTransactionState.Completed, transaction.State);
		Assert.Equal(12m, transaction.AmountReceived.Amount);
		_ = Assert.NotNull(transaction.Completed);
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task GetInvoices_SupportsDocumentedOrdering()
	{
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal("?$top=20&$skip=40&$filter=state%20eq%20%27PAID%27&$orderby=created%20desc", request.RequestUri!.Query);
			return Task.FromResult(JsonResponse("{}"));
		});
		using var http = new HttpClient(handler);

		_ = await CreateClient(http).Invoices.GetInvoices("state eq 'PAID'", top: 20, skip: 40, orderBy: "created desc");
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task IssueInvoiceFor_EncodesHandle()
	{
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal("/v1/invoices/handle/alice%2Fbob%3Fref%3D1%23memo", request.RequestUri!.AbsolutePath);
			Assert.Empty(request.RequestUri.Query);
			return Task.FromResult(JsonResponse("{}"));
		});
		using var http = new HttpClient(handler);

		_ = await CreateClient(http).Invoices.IssueInvoiceFor("alice/bob?ref=1#memo", new InvoiceReq { Amount = new Money { Amount = 1, Currency = Currency.Usd } });
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task BankPaymentMethod_BsbAndOptionalRoutingAreSupported()
	{
		using var handler = new ContractHandler(async request =>
		{
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			Assert.Equal("BSB", body.RootElement.GetProperty("transferType").GetString());
			Assert.False(body.RootElement.TryGetProperty("routingNumber", out _));
			Assert.False(body.RootElement.TryGetProperty("accountType", out _));
			return JsonResponse("""{"transferType":"BSB","currency":"AUD"}""");
		});
		using var http = new HttpClient(handler);

		var result = await CreateClient(http).PaymentMethods.Create(new PaymentMethodReq
		{
			TransferType = PaymentMethodTransferTypes.BSB,
			AccountNumber = "123456",
			Beneficiaries = []
		});

		Assert.Equal(PaymentMethodTransferTypes.BSB, result.TransferType);
		Assert.Equal(Currency.Aud, result.Currency);
		Assert.Equal(4, (int)PaymentMethodTransferTypes.Undefined);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	[Trait("Cat", "Base")]
	public async Task Payment_ExposesNetworkDetailsAndLegacyFields(bool execute)
	{
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal(execute ? HttpMethod.Patch : HttpMethod.Get, request.Method);
			var response = JsonResponse("""{"state":"COMPLETED","result":"SUCCESS","delivered":"2026-01-01T00:00:00Z","lightning":{"networkFee":{"amount":"0.000001","currency":"BTC"},"preImage":"proof"},"onchain":{"txnId":"transaction-id"}}""");
			response.StatusCode = execute ? HttpStatusCode.Accepted : HttpStatusCode.OK;
			return response;
		});
		using var http = new HttpClient(handler);
		var client = CreateClient(http);

		var payment = execute ? await client.PaymentQuotes.ExecuteQuote(Guid.NewGuid()) : await client.Payments.FindPayment(Guid.NewGuid());

		Assert.Equal(0.000001m, payment.Lightning!.NetworkFee!.Amount);
		Assert.Equal("proof", payment.Lightning.PreImage);
		Assert.Equal("transaction-id", payment.Onchain!.TxnId);
		Assert.Equal(PaymentResult.Success, payment.Result);
		_ = Assert.NotNull(payment.Delivered);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	[Trait("Cat", "Base")]
	public async Task PaymentQuote_SendsTravelRuleBeneficiary(bool onchain)
	{
		using var handler = new ContractHandler(async request =>
		{
			using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
			var beneficiary = body.RootElement.GetProperty("beneficiary");
			Assert.Equal("BUSINESS", beneficiary.GetProperty("type").GetString());
			Assert.Equal("EXCHANGE_OR_PLATFORM", beneficiary.GetProperty("destinationType").GetString());
			Assert.Equal("Sample Company", beneficiary.GetProperty("businessName").GetString());
			Assert.Equal("directory-id", beneficiary.GetProperty("vaspId").GetString());
			Assert.Equal("tax-id", beneficiary.GetProperty("nationalIdentifier").GetString());
			return JsonResponse("{}");
		});
		using var http = new HttpClient(handler);
		var client = CreateClient(http);
		var beneficiary = new PaymentQuoteBeneficiary
		{
			Type = PaymentQuoteBeneficiaryType.Business,
			DestinationType = PaymentDestinationType.ExchangeOrPlatform,
			BusinessName = "Sample Company",
			VaspId = "directory-id",
			NationalIdentifier = "tax-id"
		};

		if (onchain)
			_ = await client.PaymentQuotes.CreateOnchainQuote(new OnchainPaymentQuoteReq { BtcAddress = "bc1", SourceCurrency = Currency.Btc, Amount = new MoneyWithFee { Amount = 1, Currency = Currency.Usd }, OnchainTierId = "tier", Beneficiary = beneficiary });
		else
			_ = await client.PaymentQuotes.CreateLnQuote(new LnPaymentQuoteReq { LnInvoice = "lnbc", Beneficiary = beneficiary });
	}

	[Fact]
	[Trait("Cat", "Base")]
	public async Task ReceiveRequest_ExposesOnchainAddressUri()
	{
		using var handler = RespondWith("""{"onchain":{"address":"bc1","addressUri":"bitcoin:bc1?amount=0.001","btcAmount":"0.001"}}""");
		using var http = new HttpClient(handler);

		var result = await CreateClient(http).ReceiveRequests.FindRequest(Guid.NewGuid());

		Assert.Equal("bitcoin:bc1?amount=0.001", result.Onchain!.AddressUri);
		Assert.Equal(0.001m, result.Onchain.RequestedBtcAmount);
		Assert.Equal(0.001m, result.Onchain.BtcAmount);
	}

	private static ContractHandler RespondWith(string json) => new(_ => Task.FromResult(JsonResponse(json)));

	[Theory]
	[InlineData("exchange-create", "POST", "/v1/currency-exchange-quotes")]
	[InlineData("exchange-find", "GET", "/v1/currency-exchange-quotes/{id}")]
	[InlineData("exchange-execute", "PATCH", "/v1/currency-exchange-quotes/{id}/execute")]
	[InlineData("deposit-find", "GET", "/v1/deposits/{id}")]
	[InlineData("deposit-list", "GET", "/v1/deposits?$top=100&$skip=0")]
	[InlineData("bank-find", "GET", "/v1/payment-methods/bank/{id}")]
	[InlineData("bank-delete", "DELETE", "/v1/payment-methods/bank/{id}")]
	[InlineData("bank-list", "GET", "/v1/payment-methods/bank?$top=100&$skip=0&$supportedAction=DEPOSIT")]
	[InlineData("invoice-create", "POST", "/v1/invoices")]
	[InlineData("invoice-find", "GET", "/v1/invoices/{id}")]
	[InlineData("invoice-quote", "POST", "/v1/invoices/{id}/quote")]
	[InlineData("lnurl-create", "POST", "/v1/payment-quotes/lightning/lnurl")]
	[InlineData("onchain-tiers", "POST", "/v1/payment-quotes/onchain/tiers")]
	[InlineData("rates", "GET", "/v1/rates/ticker")]
	[InlineData("receive-create", "POST", "/v1/receive-requests")]
	[InlineData("receive-find", "GET", "/v1/receive-requests/{id}")]
	[InlineData("receive-list", "GET", "/v1/receive-requests?$top=100&$skip=0")]
	[InlineData("all-receives", "GET", "/v1/receive-requests/receives?$top=100&$skip=0")]
	[InlineData("request-receives", "GET", "/v1/receive-requests/{id}/receives?$top=100&$skip=0")]
	[Trait("Cat", "Base")]
	public async Task ExistingEndpoints_MatchDocumentedMethodAndRoute(string operation, string method, string route)
	{
		var id = Guid.NewGuid();
		using var handler = new ContractHandler(request =>
		{
			Assert.Equal(method, request.Method.Method);
			Assert.Equal(route.Replace("{id}", id.ToString(), StringComparison.Ordinal), request.RequestUri!.PathAndQuery);
			if (method is "GET" or "DELETE" or "PATCH")
				Assert.Null(request.Content);
			var response = JsonResponse(operation is "rates" or "onchain-tiers" ? "[]" : "{}");
			if (operation is "exchange-execute" or "bank-delete")
			{
				response.Content = new StringContent(string.Empty);
				response.StatusCode = operation == "exchange-execute" ? HttpStatusCode.Accepted : HttpStatusCode.NoContent;
			}
			return response;
		});
		using var http = new HttpClient(handler);
		var client = CreateClient(http);
		var money = new Money { Amount = 10, Currency = Currency.Usd };

		ResponseBase result = operation switch
		{
			"exchange-create" => await client.CurrencyExchanges.CreateQuote(new CurrencyExchangeQuoteReq { Buy = Currency.Btc, Sell = Currency.Usd, Amount = new MoneyWithFee { Amount = 10, Currency = Currency.Usd } }),
			"exchange-find" => await client.CurrencyExchanges.GetQuote(id),
			"exchange-execute" => await client.CurrencyExchanges.ExecuteQuote(id),
			"deposit-find" => await client.Deposits.FindDeposit(id),
			"deposit-list" => await client.Deposits.GetDeposits(),
			"bank-find" => await client.PaymentMethods.FindPaymentMethod(id),
			"bank-delete" => await client.PaymentMethods.DeletePaymentMethod(id),
			"bank-list" => await client.PaymentMethods.GetPaymentMethods(supportedAction: "DEPOSIT"),
			"invoice-create" => await client.Invoices.IssueInvoice(new InvoiceReq { Amount = money }),
			"invoice-find" => await client.Invoices.FindInvoice(id),
			"invoice-quote" => await client.Invoices.IssueQuote(id),
			"lnurl-create" => await client.PaymentQuotes.CreateLnurlQuote(new LnurlPaymentQuoteReq { LnAddressOrUrl = "alice@example.com", SourceCurrency = Currency.Usd, Amount = money }),
			"onchain-tiers" => await client.PaymentQuotes.GetOnchainTiers(new OnchainTiersReq { BtcAddress = "bc1", Amount = money }),
			"rates" => await client.Rates.GetRatesTicker(),
			"receive-create" => await client.ReceiveRequests.Create(new ReceiveRequestReq { Bolt11 = new Bolt11ReceiveRequestReq(), Onchain = new OnchainReceiveRequestReq() }),
			"receive-find" => await client.ReceiveRequests.FindRequest(id),
			"receive-list" => await client.ReceiveRequests.GetRequests(),
			"all-receives" => await client.ReceiveRequests.GetReceives(),
			_ => await client.ReceiveRequests.GetReceives(id)
		};

		Assert.True(result.IsSuccessStatusCode);
	}

	private static HttpResponseMessage JsonResponse(string json) => new(HttpStatusCode.OK)
	{
		Content = new StringContent(json, Encoding.UTF8, "application/json")
	};

	private static StrikeClient CreateClient(HttpClient http) => new(StrikeEnvironment.Custom,
		apiKey: "offline-test", httpClientFactory: new ContractFactory(http), serverUrl: new Uri("https://strike.example/"))
	{
		ThrowOnError = true
	};

	private sealed class ContractFactory(HttpClient client) : IHttpClientFactory
	{
		public HttpClient CreateClient(string name) => client;
	}

	private sealed class ContractHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
	{
		public ContractHandler(Func<HttpRequestMessage, HttpResponseMessage> send) : this(request => Task.FromResult(send(request)))
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => send(request);
	}
}
