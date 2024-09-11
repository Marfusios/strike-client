namespace Strike.Client.Converters;

/// <inheritdoc/>
public class DecimalConverter : JsonConverter<decimal>
{
	/// <inheritdoc/>
	public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.String)
		{
			return decimal.Parse(
				reader.GetString() ?? string.Empty,
				System.Globalization.CultureInfo.InvariantCulture);
		}

		if (reader.TryGetDecimal(out var decimalValue))
		{
			return decimalValue;
		}

		throw new InvalidOperationException($"Failed to parse decimal value (received type: {reader.TokenType})");
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) =>
		writer.WriteStringValue(
			value.ToString(System.Globalization.CultureInfo.InvariantCulture));
}
