using System.Diagnostics;

namespace Strike.Client.PayoutOriginators;

/// <summary>
/// The actual source of funds for a payout.
/// </summary>
[DebuggerDisplay("Payout originator {Id} | {State}")]
public record PayoutOriginator : ResponseBase
{
	/// <summary>
	/// Payout originator ID.
	/// </summary>
	public Guid Id { get; init; }

	/// <summary>
	/// Originator review state.
	/// </summary>
	public PayoutOriginatorState State { get; init; }

	/// <summary>
	/// Time the originator was created.
	/// </summary>
	public DateTimeOffset Created { get; init; }

	/// <summary>
	/// Individual or company details.
	/// </summary>
	public PayoutOriginatorDetails Details { get; init; } = null!;
}
