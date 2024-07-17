namespace Strike.Client.Models;

/// <summary>
/// All supported fee policies
/// </summary>
public enum FeePolicy
{
	/// <summary>
	/// The fee is included in the specified amount.
	/// </summary>
	[EnumMember(Value = "INCLUSIVE")]
	Inclusive,

	/// <summary>
	/// The fee is added on top of the specified amount.
	/// </summary>
	[EnumMember(Value = "EXCLUSIVE")]
	Exclusive,

	/// <summary>
	/// <para>Catch-all for unknown values returned by Strike. If you encounter this, please check if there is a later version of the Strike.Client library.</para>
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
