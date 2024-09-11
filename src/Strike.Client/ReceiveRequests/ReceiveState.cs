namespace Strike.Client.ReceiveRequests;
public enum ReceiveState
{
	[EnumMember(Value = "PENDING")]
	Pending = 1,

	[EnumMember(Value = "COMPLETED")]
	Completed = 2,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
