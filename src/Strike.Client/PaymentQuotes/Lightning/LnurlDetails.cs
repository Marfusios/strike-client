namespace Strike.Client.PaymentQuotes.Lightning;
public record LnurlDetails : ResponseBase
{
	/// <summary>
	/// Minimum amount that can be sent in millisatoshis.
	/// </summary>
	/// <example>100000</example>
	public long MinSendable { get; init; }

	/// <summary>
	/// Maximum amount that can be sent in millisatoshis.
	/// </summary>
	/// <example>1000000000</example>
	public long MaxSendable { get; init; }

	/// <summary>
	/// Metadata to display to the user.
	/// </summary>
	/// <example>[["text/plain","Pay marfusios"],["text/identifier","marfusios@strike.me"]]</example>
	public string? Metadata { get; init; }

	/// <summary>
	/// Maximum number of characters allowed in the payment comment. If null, no comment is allowed.
	/// </summary>
	/// <example>20</example>
	public int? CommentAllowed { get; init; }

	/// <summary>
	/// Tag to identify the type of LNURL.
	/// </summary>
	/// <example>payRequest</example>
	public string? Tag { get; init; }
}
