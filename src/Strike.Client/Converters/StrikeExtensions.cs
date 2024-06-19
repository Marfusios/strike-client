namespace Strike.Client.Converters;

/// <summary>
/// Strike-specific extension methods for <see cref="JsonSerializerOptions"/>
/// </summary>
public static class StrikeExtensions
{
	/// <summary>
	/// Extension method to add Strike converters to a <see cref="JsonSerializerOptions"/> object
	/// </summary>
	/// <param name="options">The object to which to add the converters</param>
	/// <returns>The object passed in, with the Strike converters added</returns>
	public static JsonSerializerOptions AddStrikeConverters(this JsonSerializerOptions options)
	{
		options.Converters.Add(new DateOnlyConverter());
		options.Converters.Add(new DateTimeOffsetConverter());
		options.Converters.Add(new DecimalConverter());
		options.Converters.Add(new EnumConverterFactory());
		return options;
	}
}
