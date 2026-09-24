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
	/// <remarks>For counts above Int32.MaxValue, use <see cref="TotalCount"/>. Reading this legacy property then throws OverflowException.</remarks>
	[JsonIgnore]
	public int Count
	{
		get => checked((int)TotalCount);
		init => TotalCount = value;
	}

	/// <summary>
	/// Total number of records. Ignore this value when <see cref="IsCountUnknown"/> is true.
	/// </summary>
	[JsonPropertyName("count")]
	public long TotalCount { get; init; }

	/// <summary>
	/// Indicates that the server could not determine the total number of records.
	/// </summary>
	public bool IsCountUnknown { get; init; }
}
