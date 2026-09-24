using Strike.Client.Models;

namespace Strike.Client.PayoutOriginators;

/// <summary>
/// Request to create an individual payout originator.
/// </summary>
public sealed class PayoutOriginatorIndividualReq : PayoutOriginatorReq
{
	/// <inheritdoc/>
	public override BeneficiaryType Type => BeneficiaryType.Individual;

	/// <summary>
	/// Individual's date of birth. On .NET Core 3.1 this is represented by a date-only <see cref="DateTime"/>.
	/// </summary>
	public DateOnly? DateOfBirth { get; init; }
}
