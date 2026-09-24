using Strike.Client.Models;

namespace Strike.Client.PayoutOriginators;

/// <summary>
/// Details returned for an individual or company payout originator.
/// </summary>
public record PayoutOriginatorDetails
{
	/// <summary>
	/// Whether the originator is an individual or a company.
	/// </summary>
	public required BeneficiaryType Type { get; init; }

	/// <summary>
	/// Originator name.
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Originator address.
	/// </summary>
	public required Address Address { get; init; }

	/// <summary>
	/// Individual's date of birth. On .NET Core 3.1 this is represented by a date-only <see cref="DateTime"/>.
	/// </summary>
	public DateOnly? DateOfBirth { get; init; }

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
