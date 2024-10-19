namespace Tax.Calculator.Commify.Contracts.Request;

public record CreateTaxBandRequestDto
{
    public required string BandName { get; set; }
    public required int LowerSalaryRange { get; set; }
    public required int? UpperSalaryRange { get; set; }
    public required int TaxRate { get; set; }
}