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
		var paymentMethodId = Guid.Parse("2537644e-7898-4f33-8ff3-4497901ae5e6");

		var depositRequest = new DepositReq
		{
			Amount = "10",
			PaymentMethodId = paymentMethodId,
			// not setting since it's not required
			// Fee = FeePolicy.Exclusive
		};

		// Get deposit fee estimate
		var feeEstimate = await client.Deposits.GetDepositFeeEstimate(depositRequest);
		AssertStatus(feeEstimate);
		Assert.Equal(10, feeEstimate.Amount!.Amount);
		Assert.Equal(Currency.Usd, feeEstimate.Amount.Currency);
		Assert.Equal(0, feeEstimate.Fee!.Amount);
		Assert.Equal(Currency.Usd, feeEstimate.Fee.Currency);
		Assert.Equal(10, feeEstimate.Total!.Amount);
		Assert.Equal(Currency.Usd, feeEstimate.Total.Currency);
		Assert.Equal(5, feeEstimate.SettlementPeriodInDay);

		// TODO: Initiate deposit
		// var deposit = await client.Deposits.Create(depositRequest);
		// AssertStatus(deposit);
		// Assert.NotEqual(Guid.Empty, deposit.Id);
		// Assert.Equal(10, deposit.Amount!.Amount);
		// Assert.Equal(Currency.Usd, deposit.Amount.Currency);
		// Assert.Equal(DepositState.Pending, deposit.State);

		// Get deposits
		var allRequests = await client.Deposits.GetDeposits();
		AssertStatus(allRequests);
		Assert.NotEmpty(allRequests.Items);
	}
}
