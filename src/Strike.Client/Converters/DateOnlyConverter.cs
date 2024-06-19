namespace Strike.Client.Converters;

/// <inheritdoc/>
public class DateOnlyConverter : JsonConverter<DateOnly>
{
	/// <inheritdoc/>
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		DateOnly.Parse(
			reader.GetString() ?? string.Empty,
			System.Globalization.CultureInfo.InvariantCulture);

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
		writer.WriteStringValue(
			value.ToString(
				"yyyy-MM-dd",
				System.Globalization.CultureInfo.InvariantCulture));
}
