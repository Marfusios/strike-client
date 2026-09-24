namespace Strike.Client.PayoutOriginators;

/// <summary>
/// Payout originator review state.
/// </summary>
public enum PayoutOriginatorState
{
	/// <summary>
	/// A value not yet recognized by this client.
	/// </summary>
	[EnumMember(Value = "UNDEFINED")]
	Undefined,

	/// <summary>
	/// Originator is awaiting review.
	/// </summary>
	[EnumMember(Value = "PENDING_REVIEW")]
	PendingReview,

	/// <summary>
	/// Originator was approved.
	/// </summary>
	[EnumMember(Value = "APPROVED")]
	Approved,

	/// <summary>
	/// Originator was rejected.
	/// </summary>
	[EnumMember(Value = "REJECTED")]
	Rejected,

	/// <summary>
	/// Originator is inactive.
	/// </summary>
	[EnumMember(Value = "INACTIVE")]
	Inactive
}
