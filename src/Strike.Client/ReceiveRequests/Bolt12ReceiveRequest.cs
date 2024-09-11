using System.Diagnostics;

namespace Strike.Client.ReceiveRequests;

[DebuggerDisplay("Bolt12")]
public record Bolt12ReceiveRequest
{
	public required string Offer { get; init; }
}
