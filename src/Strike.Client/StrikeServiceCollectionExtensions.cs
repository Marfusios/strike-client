namespace Strike.Client;

/// <summary>
/// Holding class for extension methods.
/// </summary>
public static class StrikeServiceCollectionExtensions
{
	/// <summary>
	/// Registers all the Strike infrastructure, including <see cref="StrikeOptions"/> 
	/// configuration data stored in the <see cref="StrikeOptions.SectionKey"/> (<c>"Strike"</c>)
	/// section of the provided configuration root.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
	/// <param name="configurationRoot">The configuration root that holds a <c>"Strike"</c> section of configuration data for Strike</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
	public static IServiceCollection AddStrike(this IServiceCollection services, IConfigurationRoot configurationRoot) =>
		services.AddStrike(configurationRoot.GetSection(StrikeOptions.SectionKey));

	/// <summary>
	/// Registers all the Strike infrastructure, including <see cref="StrikeOptions"/> 
	/// configuration data stored in the provided configuration section.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
	/// <param name="configuration">The configuration being bound</param>
	/// <param name="handler">Optional custom HTTP handler</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
	public static IServiceCollection AddStrike(this IServiceCollection services, IConfiguration configuration, HttpClientHandler? handler = null) =>
		services
			.Configure<StrikeOptions>(configuration)
			.AddStrikeHttpClient(handler)
			.AddStrikeClient();

	/// <summary>
	/// Registers all the Strike infrastructure
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
	/// <param name="optionsAction">Custom options configuration callback</param>
	/// <param name="handler">Optional custom HTTP handler</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
	public static IServiceCollection AddStrike(this IServiceCollection services, Action<StrikeOptions> optionsAction, HttpClientHandler? handler = null) =>
		services
			.Configure(optionsAction)
			.AddStrikeHttpClient(handler)
			.AddStrikeClient();

	/// <summary>
	/// Registers the <see cref="StrikeClient"/> as a singleton for use in the DI system.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
	public static IServiceCollection AddStrikeClient(this IServiceCollection services) => services.AddTransient<StrikeClient>();

	/// <summary>
	/// Registers a <c>"StrikeClient"</c> named HttpClient configured for Strike usage.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
	/// <param name="handler">Optional custom HTTP handler</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
	public static IServiceCollection AddStrikeHttpClient(this IServiceCollection services, HttpClientHandler? handler = null)
	{
		services
			.AddHttpClient(StrikeOptions.HttpClientName)
			.ConfigurePrimaryHttpMessageHandler(() =>
				handler ?? new HttpClientHandler
				{
#if NETCOREAPP3_1_OR_GREATER
					AutomaticDecompression = DecompressionMethods.All,
#else
					AutomaticDecompression =
						DecompressionMethods.GZip
						| DecompressionMethods.Deflate,
#endif
				});
		return services;
	}
}
