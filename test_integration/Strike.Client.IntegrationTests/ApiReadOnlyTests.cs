namespace Strike.Client.IntegrationTests;

/// <summary>Read-only smoke tests for API areas covered by the documentation audit.</summary>
public class ApiReadOnlyTests : TestsBase
{
	[SkippableTheory]
	[InlineData("limits")]
	[InlineData("invoices")]
	[InlineData("deposits")]
	[InlineData("payment-methods")]
	[InlineData("payouts")]
	[InlineData("payout-originators")]
	[InlineData("events")]
	[InlineData("subscriptions")]
	[InlineData("receive-requests")]
	[InlineData("receives")]
	public async Task ListOrRead_ShouldWork(string api)
	{
		var client = GetClient(readOnly: true);
		ResponseBase response = api switch
		{
			"limits" => await client.Accounts.GetLimits(),
			"invoices" => await client.Invoices.GetInvoices(top: 1),
			"deposits" => await client.Deposits.GetDeposits(top: 1),
			"payment-methods" => await client.PaymentMethods.GetPaymentMethods(top: 1),
			"payouts" => await client.Payouts.GetPayouts(top: 1),
			"payout-originators" => await client.PayoutOriginators.GetPayoutOriginators(top: 1),
			"events" => await client.Events.GetEvents(top: 1),
			"subscriptions" => await client.Subscriptions.GetSubscriptions(),
			"receive-requests" => await client.ReceiveRequests.GetRequests(top: 1),
			"receives" => await client.ReceiveRequests.GetReceives(top: 1),
			_ => throw new ArgumentException("Unknown API group.", nameof(api))
		};

		Skip.If(api == "limits" && response.StatusCode == System.Net.HttpStatusCode.Forbidden,
			"Account limits require the partner.account.profile.read-own scope, which is not available to this key.");
		AssertStatus(response);
	}
}
