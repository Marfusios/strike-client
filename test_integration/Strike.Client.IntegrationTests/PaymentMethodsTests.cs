using Strike.Client.Models;
using Strike.Client.PaymentMethods;

namespace Strike.Client.IntegrationTests;

public class PaymentMethodsTests : TestsBase
{
	[SkippableFact]
	public async Task PaymentMethodsFlow_ShouldWork()
	{
		var client = GetClient();

		// Create payment method
		var request = new PaymentMethodReq
		{
			AccountNumber = "1111222233334444",
			RoutingNumber = "071004200",
			TransferType = PaymentMethodTransferTypes.ACH,
			Beneficiaries = [
				new Beneficiary
				{
					Name = "Test User",
					Type = BeneficiaryType.Individual
				}
			]
		};
		var created = await client.PaymentMethods.Create(request);
		AssertStatus(created);

		// Get all payment methods
		var allRequests = await client.PaymentMethods.GetPaymentMethods();
		AssertStatus(allRequests);
		Assert.NotEmpty(allRequests.Items);

		var found = allRequests.Items.FirstOrDefault(x => x.Id == created.Id);
		Assert.NotNull(found);

		var foundRequest = await client.PaymentMethods.FindPaymentMethod(found.Id);
		AssertStatus(foundRequest);

		// Delete payment method
		var deleted = await client.PaymentMethods.DeletePaymentMethod(foundRequest.Id);
		AssertStatus(deleted);
	}
}
