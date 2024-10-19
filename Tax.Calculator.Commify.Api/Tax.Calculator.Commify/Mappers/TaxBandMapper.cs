using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Mappers;

public static class TaxBandMapper
{
    public static TaxBand MapToTaxBand(CreateTaxBandRequestDto request)
    {
        return new TaxBand
        {
            BandName = request.BandName,
            LowerSalaryRange = request.LowerSalaryRange,
            UpperSalaryRange = request.UpperSalaryRange,
            TaxRate = request.TaxRate,
        };
    }
}