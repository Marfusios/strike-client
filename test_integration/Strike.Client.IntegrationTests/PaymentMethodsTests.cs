using Strike.Client.Models;
using Strike.Client.ReceiveRequests;
using Strike.Client.ReceiveRequests.Requests;

namespace Strike.Client.IntegrationTests;
public class PaymentMethodsTests : TestsBase
{
	[SkippableFact]
	public async Task PaymentMethodsFlow_ShouldWork()
	{
		var client = GetClient();

		// TODO: Add creation of a payment method
		
		// Get all payment methods
		var allRequests = await client.PaymentMethods.GetPaymentMethods();
		AssertStatus(allRequests);
		Assert.NotEmpty(allRequests.Items);

		var first = allRequests.Items.First();
		var foundRequest = await client.PaymentMethods.FindPaymentMethod(first.Id);
		AssertStatus(foundRequest);

		// TODO: Delete payment method
	}
}
