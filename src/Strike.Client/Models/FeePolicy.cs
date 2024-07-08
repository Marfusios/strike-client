namespace Strike.Client.Models;

/// <summary>
/// All supported fee policies
/// </summary>
public enum FeePolicy
{
	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "INCLUSIVE")]
	Inclusive,

	/// <summary>
	/// 
	/// </summary>
	[EnumMember(Value = "EXCLUSIVE")]
	Exclusive,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
