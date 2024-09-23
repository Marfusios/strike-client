using Strike.Client.Models;
using Strike.Client.ReceiveRequests;
using Strike.Client.ReceiveRequests.Requests;

namespace Strike.Client.IntegrationTests;
public class ReceiveRequestsTests : TestsBase
{
	[SkippableFact]
	public async Task ReceiveRequestFlow_ShouldWork()
	{
		var client = GetClient();

		var receiveRequest = await client.ReceiveRequests.Create(new ReceiveRequestReq
		{
			TargetCurrency = Currency.Usd,
			Bolt11 = new Bolt11ReceiveRequestReq
			{
				Amount = new Money { Amount = 1m, Currency = Currency.Eur },
				Description = "Receive request from Strike .NET client",
				ExpiryInSeconds = 60 * 10
			},
			Onchain = new OnchainReceiveRequestReq
			{
				Amount = new Money { Amount = 1m, Currency = Currency.Eur }
			},
			//Bolt12 = new Bolt12ReceiveRequestReq()
		});
		AssertStatus(receiveRequest);
		AssertReceiveRequest(receiveRequest);
		var bolt11 = receiveRequest.Bolt11;

		var foundRequest = await client.ReceiveRequests.FindRequest(receiveRequest.ReceiveRequestId);
		AssertStatus(foundRequest);
		AssertReceiveRequest(foundRequest);

		var allRequests = await client.ReceiveRequests.GetRequests();
		AssertStatus(allRequests);
		Assert.NotEmpty(allRequests.Items);

		var requestByInvoice = await client.ReceiveRequests.GetRequests(bolt11Invoice: [bolt11?.Invoice]);
		AssertStatus(requestByInvoice);
		Assert.NotNull(requestByInvoice);
		var requestByInvoiceItem = Assert.Single(requestByInvoice.Items);
		Assert.Equal(bolt11?.PaymentHash, requestByInvoiceItem.Bolt11?.PaymentHash);
		AssertReceiveRequest(requestByInvoiceItem);

		var requestByPaymentHash = await client.ReceiveRequests.GetRequests(paymentHash: [bolt11?.PaymentHash]);
		AssertStatus(requestByPaymentHash);
		Assert.NotNull(requestByPaymentHash);
		var requestByPaymentHashItem = Assert.Single(requestByPaymentHash.Items);
		Assert.Equal(bolt11?.PaymentHash, requestByPaymentHashItem.Bolt11?.PaymentHash);
		AssertReceiveRequest(requestByPaymentHashItem);

		var receives = await client.ReceiveRequests.GetReceives(receiveRequest.ReceiveRequestId);
		AssertStatus(receives);
		Assert.NotNull(receives);
		Assert.Empty(receives.Items);
	}

	private static void AssertReceiveRequest(ReceiveRequest? receiveRequest)
	{
		Assert.NotNull(receiveRequest);
		Assert.Equal(Currency.Usd, receiveRequest.TargetCurrency);

		Assert.NotNull(receiveRequest.Bolt11);
		Assert.NotNull(receiveRequest.Onchain);
		//Assert.NotNull(receiveRequest.Bolt12);

		Assert.Equal(1m, receiveRequest.Bolt11.RequestedAmount?.Amount);
		Assert.Equal(1m, receiveRequest.Onchain.RequestedAmount?.Amount);

		//Assert.False(string.IsNullOrEmpty(receiveRequest.Bolt12.Offer));
		Assert.False(string.IsNullOrEmpty(receiveRequest.Bolt11.Description));
		Assert.False(string.IsNullOrEmpty(receiveRequest.Bolt11.Invoice));
		Assert.False(string.IsNullOrEmpty(receiveRequest.Onchain.Address));

		Assert.True(receiveRequest.Bolt11.BtcAmount > 0);
		Assert.True(receiveRequest.Onchain.BtcAmount > 0);
	}
}
