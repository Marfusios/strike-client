﻿using Strike.Client.Models;

namespace Strike.Client;

public sealed partial class StrikeClient
{
	/// <summary>
	/// Rates API endpoints
	/// </summary>
	public RatesClient Rates => new(this);

	public record RatesClient(StrikeClient Client)
	{
		/// <summary>
		/// Get account balance details
		/// </summary>
		public Task<ResponseCollection<ConversionAmount>> GetRatesTicker() =>
			Client.GetAsync("/v1/rates/ticker")
				.ParseResponseAsync<ResponseCollection<ConversionAmount>>();
	}
}
