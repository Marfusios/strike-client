namespace Strike.Client.CurrencyExchanges;

/// <summary>
/// Quote result state
/// </summary>
public enum CurrencyExchangeState
{
	/// <summary>
	/// The quote is new
	/// </summary>
	[EnumMember(Value = "NEW")]
	NEW,

	/// <summary>
	/// The quote is pending
	/// </summary>
	[EnumMember(Value = "PENDING")]
	Pending,

	/// <summary>
	/// The quote has been completed
	/// </summary>
	[EnumMember(Value = "COMPLETED")]
	Completed,

	/// <summary>
	/// The quote has failed
	/// </summary>
	[EnumMember(Value = "FAILED")]
	Failed,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
