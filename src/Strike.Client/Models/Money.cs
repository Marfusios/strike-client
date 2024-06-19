using System.Diagnostics;

namespace Strike.Client.Models;

/// <summary>
/// Money object
/// </summary>
[DebuggerDisplay("{Amount} {Currency}")]
public class Money
{
	/// <summary>
	/// <para>Currency amount in decimal format</para>
	/// </summary>
	public decimal Amount { get; set; }

	/// <summary>
	/// <para>Currency code</para>
	/// </summary>
	public required Currency Currency { get; set; }
}
