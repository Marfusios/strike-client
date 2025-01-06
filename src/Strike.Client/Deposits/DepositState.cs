namespace Strike.Client.Deposits;

/// <summary>
/// Payment method state
/// </summary>
public enum DepositState
{
	/// <summary>
	/// NEW
	/// </summary>
	[EnumMember(Value = "NEW")]
	New,

	/// <summary>
	/// PENDING
	/// </summary>
	[EnumMember(Value = "PENDING")]
	Pending,

	/// <summary>
	/// COMPLETED
	/// </summary>
	[EnumMember(Value = "COMPLETED")]
	Completed,

	/// <summary>
	/// FAILED
	/// </summary>
	[EnumMember(Value = "FAILED")]
	Failed,

	/// <summary>
	/// REVERSED
	/// </summary>
	[EnumMember(Value = "REVERSED")]
	Reversed,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
