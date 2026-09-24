using Strike.Client.Models;

namespace Strike.Client.PayoutOriginators;

/// <summary>
/// Common fields for creating an individual or company payout originator.
/// </summary>
public abstract class PayoutOriginatorReq : RequestBase
{
	/// <summary>
	/// Originator discriminator, supplied by the concrete request type.
	/// </summary>
	public abstract BeneficiaryType Type { get; }

	/// <summary>
	/// Originator name.
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Originator address.
	/// </summary>
	public required Address Address { get; init; }
}
