namespace Strike.Client.Subscriptions;

/// <summary>
/// Supported webhook payload formats.
/// </summary>
public enum WebhookVersion
{
	/// <summary>
	/// The first webhook payload format.
	/// </summary>
	[EnumMember(Value = "v1")]
	V1,

	/// <summary>
	/// The second webhook payload format.
	/// </summary>
	[EnumMember(Value = "v2")]
	V2,

	/// <summary>
	/// An unknown version returned by Strike.
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined
}
