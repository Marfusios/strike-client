namespace Strike.Client.ReceiveRequests;
public enum ReceiveType
{
	[EnumMember(Value = "LIGHTNING")]
	Lightning,

	[EnumMember(Value = "ONCHAIN")]
	Onchain,

	[EnumMember(Value = "P2P")]
	P2P,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
