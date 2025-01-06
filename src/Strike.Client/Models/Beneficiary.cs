using System.Diagnostics;

namespace Strike.Client.Models;

[DebuggerDisplay("Beneficiery {Name}, {Type}")]
public record Beneficiary
{
	/// <summary>
	/// Date of birth
	/// </summary>
	public DateTimeOffset? DateOfBirth { get; init; }

	/// <summary>
	/// Country
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Type of beneficiary
	/// </summary>
	public required BeneficiaryType Type { get; init; }

	/// <summary>
	/// Address
	/// </summary>
	public Address? Address { get; init; }

	// company specific fields

	/// <summary>
	/// Email
	/// </summary>
	public string? Email { get; init; }

	/// <summary>
	/// Phone Number
	/// </summary>
	public string? PhoneNumber { get; init; }

	/// <summary>
	/// Phone Number
	/// </summary>
	public string? Url { get; init; }
}

public enum BeneficiaryType
{
	/// <summary>
	/// INDIVIDUAL
	/// </summary>
	[EnumMember(Value = "INDIVIDUAL")]
	Individual,

	/// <summary>
	/// COMPANY
	/// </summary>
	[EnumMember(Value = "COMPANY")]
	Company,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
