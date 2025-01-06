using System.Diagnostics;

namespace Strike.Client.Models;

[DebuggerDisplay("Address {Line1}, {City}, {PostCode} {State}, {Country}")]
public record Address
{
	/// <summary>
	/// Country
	/// </summary>
	public required string Country { get; init; }

	/// <summary>
	/// State
	/// </summary>
	public string? State { get; init; }

	/// <summary>
	/// City
	/// </summary>
	public required string City { get; init; }

	/// <summary>
	/// PostCode
	/// </summary>
	public required string PostCode { get; init; }

	/// <summary>
	/// Line1
	/// </summary>
	public required string Line1 { get; init; }
}
