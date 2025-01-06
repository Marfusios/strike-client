using Strike.Client.Deposits;
using Strike.Client.Models;
using Strike.Client.PaymentMethods;

namespace Strike.Client.IntegrationTests;
public class DepositsTests : TestsBase
{
	[SkippableFact]
	public async Task DepositsFlow_ShouldWork()
	{
		var client = GetClient();

		var allMethods = await client.PaymentMethods.GetPaymentMethods();
		AssertStatus(allMethods);

		var paymentMethod = allMethods.Items.FirstOrDefault(x => x is { State: PaymentMethodState.Ready, TransferType: PaymentMethodTransferTypes.ACH });
		Skip.If(paymentMethod == null, "There is no active payment method for ACH, create it first!");

		var depositRequest = new DepositReq
		{
			Amount = "10",
			PaymentMethodId = paymentMethod.Id,
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
		//Assert.Equal(5, feeEstimate.SettlementPeriodInDay);

		// Initiate deposit
		var deposit = await client.Deposits.Create(depositRequest);
		AssertStatus(deposit);
		Assert.NotEqual(Guid.Empty, deposit.Id);
		Assert.Equal(10, deposit.Amount!.Amount);
		Assert.Equal(Currency.Usd, deposit.Amount.Currency);
		Assert.Equal(DepositState.Pending, deposit.State);

		// Get deposits
		var allRequests = await client.Deposits.GetDeposits();
		AssertStatus(allRequests);
		Assert.NotEmpty(allRequests.Items);
	}
}
