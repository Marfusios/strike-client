using System.Diagnostics;

namespace Strike.Client.Models;

/// <summary>
/// Money object with fee policy
/// </summary>
[DebuggerDisplay("{Amount} {Currency}, fee: {FeePolicy}")]
public class MoneyWithFee : Money
{
	/// <summary>
	/// <para>Should the fee be included in the amount or added on top of it. Optional param, usually the default is EXCLUSIVE</para>
	/// </summary>
	public FeePolicy? FeePolicy { get; set; }
}
