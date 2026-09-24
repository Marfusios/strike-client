using Strike.Client.Models;

namespace Strike.Client.Accounts;

/// <summary>Limits for the authenticated account. Unconfigured limit categories are null.</summary>
public record AccountLimits : ResponseBase
{
	/// <summary>General deposit limits.</summary>
	public TransactionLimits? Deposit { get; init; }

	/// <summary>ACH deposit limits, when configured separately.</summary>
	public TransactionLimits? AchDeposit { get; init; }

	/// <summary>Outgoing transaction limits, excluding bank transfers.</summary>
	public TransactionLimits? Send { get; init; }

	/// <summary>Bank transfer limits.</summary>
	public TransactionLimits? Withdrawal { get; init; }

	/// <summary>Limits on spending a pending balance.</summary>
	public TransactionLimits? Instant { get; init; }
}

/// <summary>Limits on transaction count and total value.</summary>
public record TransactionLimits
{
	/// <summary>Limits on the number of transactions.</summary>
	public TransactionCountLimits? Count { get; init; }

	/// <summary>Limits on the value of transactions.</summary>
	public TransactionAmountLimits? Amount { get; init; }
}

/// <summary>Transaction count limits by period.</summary>
public record TransactionCountLimits
{
	/// <summary>Currently effective limit.</summary>
	public int Effective { get; init; }

	/// <summary>Lifetime limit and usage.</summary>
	public CountLimit? Lifetime { get; init; }

	/// <summary>Calendar year limit and usage.</summary>
	public CountLimit? CalendarYear { get; init; }

	/// <summary>Limits and usage for rolling periods.</summary>
	public IReadOnlyCollection<CountLimitCycle>? Cycles { get; init; }
}

/// <summary>A transaction count limit and its usage.</summary>
public record CountLimit
{
	/// <summary>Allowed transaction count.</summary>
	public int Limit { get; init; }

	/// <summary>Transaction count already used.</summary>
	public int Used { get; init; }
}

/// <summary>A transaction count limit for a rolling period.</summary>
public record CountLimitCycle : CountLimit
{
	/// <summary>Length of the period in days.</summary>
	public int Days { get; init; }
}

/// <summary>Transaction amount limits by period.</summary>
public record TransactionAmountLimits
{
	/// <summary>Currently effective amount limit.</summary>
	public Money Effective { get; init; } = default!;

	/// <summary>Limit for one transaction.</summary>
	[JsonPropertyName("single")]
	public Money? SingleTransaction { get; init; }

	/// <summary>Lifetime limit and usage.</summary>
	public AmountLimit? Lifetime { get; init; }

	/// <summary>Calendar year limit and usage.</summary>
	public AmountLimit? CalendarYear { get; init; }

	/// <summary>Limits and usage for rolling periods.</summary>
	public IReadOnlyCollection<AmountLimitCycle>? Cycles { get; init; }
}

/// <summary>A transaction amount limit and its usage.</summary>
public record AmountLimit
{
	/// <summary>Allowed transaction amount, when configured.</summary>
	public Money? Limit { get; init; }

	/// <summary>Amount already used, when available.</summary>
	public Money? Used { get; init; }
}

/// <summary>A transaction amount limit for a rolling period.</summary>
public record AmountLimitCycle : AmountLimit
{
	/// <summary>Length of the period in days.</summary>
	public int Days { get; init; }
}
