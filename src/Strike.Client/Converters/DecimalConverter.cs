namespace Strike.Client.Converters;

/// <inheritdoc/>
public class DecimalConverter : JsonConverter<decimal>
{
	/// <inheritdoc/>
	public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		decimal.Parse(
			reader.GetString() ?? string.Empty,
			System.Globalization.CultureInfo.InvariantCulture);

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) =>
		writer.WriteStringValue(
			value.ToString(System.Globalization.CultureInfo.InvariantCulture));
}
