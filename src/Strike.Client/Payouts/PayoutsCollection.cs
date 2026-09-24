namespace Strike.Client.Payouts;

/// <summary>
/// A page of payouts.
/// </summary>
public record PayoutsCollection : ResponseBase
{
	/// <summary>
	/// Payouts in this page.
	/// </summary>
	public IReadOnlyCollection<Payout> Items { get; init; } = [];

	/// <summary>
	/// Total number of records. Ignore when <see cref="IsCountUnknown"/> is true.
	/// </summary>
	public long Count { get; init; }

	/// <summary>
	/// Whether Strike could not determine the total count.
	/// </summary>
	public bool IsCountUnknown { get; init; }
}
