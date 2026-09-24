namespace Strike.Client.Payouts;

/// <summary>
/// Payout lifecycle state.
/// </summary>
public enum PayoutState
{
	/// <summary>
	/// A value not yet recognized by this client.
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined,

	/// <summary>
	/// Payout created.
	/// </summary>
	[EnumMember(Value = "NEW")]
	New,

	/// <summary>
	/// Payout initiated.
	/// </summary>
	[EnumMember(Value = "INITIATED")]
	Initiated,

	/// <summary>
	/// Payout completed.
	/// </summary>
	[EnumMember(Value = "COMPLETED")]
	Completed,

	/// <summary>
	/// Payout failed.
	/// </summary>
	[EnumMember(Value = "FAILED")]
	Failed,

	/// <summary>
	/// Payout reversed.
	/// </summary>
	[EnumMember(Value = "REVERSED")]
	Reversed
}
