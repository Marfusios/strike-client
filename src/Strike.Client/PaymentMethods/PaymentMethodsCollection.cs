using System.Diagnostics;

namespace Strike.Client.PaymentMethods;

[DebuggerDisplay("PaymentMethods {Count}")]
public record PaymentMethodsCollection : ResponseBase
{
	/// <summary>
	/// The page items.
	/// </summary>
	public IReadOnlyCollection<PaymentMethod> Items { get; init; } = [];

	/// <summary>
	/// Total number of records
	/// </summary>
	public int Count { get; init; }
}
