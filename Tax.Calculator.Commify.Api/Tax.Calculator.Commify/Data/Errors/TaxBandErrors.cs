using ErrorOr;

namespace Tax.Calculator.Commify.Data.Errors;

public static partial class Errors
{
    public static class TaxBand
    {
        public static Error GapBetweenTaxBands = Error.Validation(description: "The Tax Bands cannot have gaps between them.");
        public static Error OverlappingTaxBands = Error.Validation(description: "The Tax Bands cannot have overlapping salary ranges between them.");
        public static Error MultipleInfiniteUpperSalaryRanges = Error.Validation(description: "The Tax Bands cannot have multiple infinite salary ranges.");
        public static Error BandWithInfiniteAmmountExists = Error.Validation(description: "The Tax Bands already has a tax band with an infinite ammount.");
    }
}