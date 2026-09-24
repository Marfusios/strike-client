namespace Strike.Client.PayoutOriginators;

/// <summary>
/// A page of payout originators.
/// </summary>
public record PayoutOriginatorsCollection : ResponseBase
{
	/// <summary>
	/// Originators in this page.
	/// </summary>
	public IReadOnlyCollection<PayoutOriginator> Items { get; init; } = [];

	/// <summary>
	/// Total number of records. Ignore when <see cref="IsCountUnknown"/> is true.
	/// </summary>
	public long Count { get; init; }

	/// <summary>
	/// Whether Strike could not determine the total count.
	/// </summary>
	public bool IsCountUnknown { get; init; }
}
