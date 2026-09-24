using Strike.Client.Models;

namespace Strike.Client.PayoutOriginators;

/// <summary>
/// Request to create a company payout originator.
/// </summary>
public sealed class PayoutOriginatorCompanyReq : PayoutOriginatorReq
{
	/// <inheritdoc/>
	public override BeneficiaryType Type => BeneficiaryType.Company;

	/// <summary>
	/// Company's email address.
	/// </summary>
	public string? Email { get; init; }

	/// <summary>
	/// Company's phone number.
	/// </summary>
	public string? PhoneNumber { get; init; }

	/// <summary>
	/// Company's website URL.
	/// </summary>
	public string? Url { get; init; }
}
