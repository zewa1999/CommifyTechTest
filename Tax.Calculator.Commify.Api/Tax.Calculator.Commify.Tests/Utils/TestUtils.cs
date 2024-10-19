using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Tests.Utils;

internal static class TestUtils
{
    public static List<TaxBand> TaxBandsData = new List<TaxBand>
        {
            new TaxBand
            {
                Id = Guid.NewGuid(),
                BandName = "Tax Band C",
                LowerSalaryRange = 20000,
                UpperSalaryRange = null,
                TaxRate = 40
            },
            new TaxBand
            {
                Id = Guid.NewGuid(),
                BandName = "Tax Band B",
                LowerSalaryRange = 5000,
                UpperSalaryRange = 20000,
                TaxRate = 20
            },
            new TaxBand
            {
                Id = Guid.NewGuid(),
                BandName = "Tax Band A",
                LowerSalaryRange = 0,
                UpperSalaryRange = 5000,
                TaxRate = 0
            }
        };

    public static TaxBand CreateTaxBand(int lowerSalaryRange, int? upperSalaryRange, string bandName = "Test Band", int taxRate = 10)
    {
        return new TaxBand
        {
            Id = Guid.NewGuid(),
            BandName = bandName,
            LowerSalaryRange = lowerSalaryRange,
            UpperSalaryRange = upperSalaryRange,
            TaxRate = taxRate,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };
    }
}