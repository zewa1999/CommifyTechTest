namespace Tax.Calculator.Commify.Data.Entities;

public class TaxBand : IBaseEntity
{
    public Guid Id { get; set; }
    public required string BandName { get; set; }
    public required int LowerSalaryRange { get; set; }
    public required int? UpperSalaryRange { get; set; }
    public required int TaxRate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}