using System.Globalization;
using System.Text.Json;
using Strike.Client.Converters;
using Strike.Client.Models;

namespace Strike.Client.Tests;

public class ConverterTests
{
	private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddStrikeConverters();

	[Theory]
	[InlineData("\"0.12345678\"")]
	[InlineData("0.12345678")]
	public void Decimal_ParsesStringsAndNumbersRegardlessOfCulture(string json)
	{
		var originalCulture = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");

			Assert.Equal(0.12345678m, JsonSerializer.Deserialize<decimal>(json, Options));
			Assert.Equal("\"0.12345678\"", JsonSerializer.Serialize(0.12345678m, Options));
		}
		finally
		{
			CultureInfo.CurrentCulture = originalCulture;
		}
	}

	[Theory]
	[InlineData("\"BTC\"", Currency.Btc)]
	[InlineData("\"usd\"", Currency.Usd)]
	[InlineData("\"FUTURE_CURRENCY\"", Currency.Undefined)]
	[InlineData("1", Currency.Usd)]
	public void Enum_ParsesNamesNumbersAndUnknownValues(string json, Currency expected)
	{
		Assert.Equal(expected, JsonSerializer.Deserialize<Currency>(json, Options));
	}

	[Fact]
	public void Enum_WritesWireNameAndSupportsNullableValues()
	{
		Assert.Equal("\"BTC\"", JsonSerializer.Serialize(Currency.Btc, Options));
		Assert.Equal("null", JsonSerializer.Serialize<Currency?>(null, Options));
		Assert.Null(JsonSerializer.Deserialize<Currency?>("null", Options));
		Assert.Equal(Currency.Usd, JsonSerializer.Deserialize<Currency?>("\"USD\"", Options));
	}

	[Fact]
	public void DateConverters_RoundTripDateAndNormalizeTimestampToUtc()
	{
		var date = new DateOnly(2026, 9, 24);
		var timestamp = new DateTimeOffset(2026, 9, 24, 12, 30, 45, TimeSpan.FromHours(2));

		Assert.Equal("\"2026-09-24\"", JsonSerializer.Serialize(date, Options));
		Assert.Equal(date, JsonSerializer.Deserialize<DateOnly>("\"2026-09-24\"", Options));
		Assert.Equal("\"2026-09-24T10:30:45Z\"", JsonSerializer.Serialize(timestamp, Options));
		Assert.Equal(timestamp, JsonSerializer.Deserialize<DateTimeOffset>("\"2026-09-24T10:30:45Z\"", Options));
	}
}
