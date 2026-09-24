namespace Strike.Client.PaymentQuotes;

/// <summary>
/// Beneficiary details used by Strike for Lightning and onchain payment quotes.
/// </summary>
public class PaymentQuoteBeneficiary
{
	/// <summary>
	/// Legacy indication of whether the destination belongs to the sender.
	/// </summary>
	public bool? IsOwnDestination { get; init; }

	public PaymentQuoteBeneficiaryType? Type { get; init; }
	public PaymentDestinationType? DestinationType { get; init; }
	public string? VaspId { get; init; }
	public string? VaspName { get; init; }
	public string? BusinessName { get; init; }
	public string? IndividualFirstName { get; init; }
	public string? IndividualLastName { get; init; }
	public string? NationalIdentifier { get; init; }
}
