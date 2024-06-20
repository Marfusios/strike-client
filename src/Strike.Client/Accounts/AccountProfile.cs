using System.Diagnostics;

namespace Strike.Client.Accounts;

/// <summary>
/// Account profile details
/// </summary>
[DebuggerDisplay("Account {Handle}")]
public record AccountProfile : ResponseBase
{
	/// <summary>
	/// Account ID
	/// </summary>
	/// <example>e8717a40-0506-485c-b10c-72d2a955d873</example>
	public Guid Id { get; init; }

	/// <summary>
	/// Account handle
	/// </summary>
	public string Handle { get; init; } = default!;

	/// <summary>
	/// Account LN address
	/// </summary>
	public string LnAddress => $"{Handle}@strike.me";

	/// <summary>
	/// Account avatar URL
	/// </summary>
	public string? AvatarUrl { get; init; }

	/// <summary>
	/// Account description
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Account can receive funds
	/// </summary>
	/// <example>true</example>
	public bool CanReceive { get; init; }

	/// <summary>
	/// Currencies supported for the account
	/// </summary>
	public IReadOnlyCollection<AccountCurrency> Currencies { get; init; } = [];
}
