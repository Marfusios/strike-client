using System.Diagnostics;

namespace Strike.Client.Models;

/// <summary>
/// Conversion amount details
/// </summary>
[DebuggerDisplay("{Amount} from {SourceCurrency} to {TargetCurrency}")]
public class ConversionAmount
{
	/// <summary>
	/// <para>Conversion amount in decimal format</para>
	/// </summary>
	public decimal Amount { get; set; }

	/// <summary>
	/// <para>Source currency code</para>
	/// </summary>
	public required Currency SourceCurrency { get; set; }

	/// <summary>
	/// <para>Target currency code</para>
	/// </summary>
	public required Currency TargetCurrency { get; set; }
}
