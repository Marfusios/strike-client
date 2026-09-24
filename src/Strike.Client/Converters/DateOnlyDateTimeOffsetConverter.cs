using System.Globalization;

namespace Strike.Client.Converters;

/// <summary>Maps a date-only API field to an existing DateTimeOffset property.</summary>
public sealed class DateOnlyDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
	/// <inheritdoc/>
	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		DateTimeOffset.Parse(reader.GetString() ?? string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
}
