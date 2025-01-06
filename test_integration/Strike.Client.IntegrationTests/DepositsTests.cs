using Strike.Client.Deposits;
using Strike.Client.Models;

namespace Strike.Client.IntegrationTests;
public class DepositsTests : TestsBase
{
	[SkippableFact]
	public async Task DepositsFlow_ShouldWork()
	{
		var client = GetClient();

		// TODO: Set your own PaymentMethodId
		// give me guid with zeros
		var paymentMethodId = Guid.Parse("00000000-0000-0000-0000-000000000000");

		var depositRequest = new DepositReq
		{
			Amount = new Money
			{
				Currency = Currency.Usd,
				Amount = 10
			},
			PaymentMethodId = paymentMethodId,
			// not setting since it's not required
			// Fee = FeePolicy.Exclusive
		};

		// Get deposit fee estimate
		var feeEstimate = await client.Deposits.GetDepositFeeEstimate(depositRequest);
		AssertStatus(feeEstimate);
		Assert.Equal(10, feeEstimate.Amount!.Amount);
		Assert.Equal(Currency.Usd, feeEstimate.Amount.Currency);

		// TODO: Initiate deposit
		// var deposit = await client.Deposits.Create(depositRequest);
		// AssertStatus(deposit);
		// Assert.Equal(10, deposit.Amount!.Amount);
		// Assert.Equal(Currency.Usd, deposit.Amount.Currency);
		// Assert.Equal(DepositState.Pending, deposit.State);

		// Get deposits
		var allRequests = await client.Deposits.GetDeposits();
		AssertStatus(allRequests);
		Assert.NotEmpty(allRequests.Items);
	}
}
