using System.Diagnostics;
using Strike.Client.Models;

namespace Strike.Client.CurrencyExchanges;

[DebuggerDisplay("Quote {Source} --> {Target}")]
public record CurrencyExchangeQuote : ResponseBase
{
	/// <summary>
	/// Unique identifier
	/// </summary>
	public Guid Id { get; init; }

	/// <summary>
	/// Time at which quote was created
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// Time at which quote is expiring
	/// </summary>
	public DateTimeOffset ValidUntil { get; init; }

	public Money Source { get; init; } = default!;

	public Money Target { get; init; } = default!;

	public Money Fee { get; init; } = default!;

	/// <summary>
	/// Applied source -> target currency conversion rate
	/// </summary>
	public ConversionAmount ConversionRate { get; init; } = default!;

	public CurrencyExchangeState State { get; init; }

	public DateTimeOffset? Completed { get; init; }
}
