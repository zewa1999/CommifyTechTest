using ErrorOr;
using Tax.Calculator.Commify.Data.Entities;
using Tax.Calculator.Commify.Data.Errors;
using Tax.Calculator.Commify.Data.Repositories;

namespace Tax.Calculator.Commify.Business;

public class TaxBandService : ITaxBandService
{
    private readonly ITaxBandRepository _taxBandRepository;
    private readonly ILogger<TaxBandService> _logger;

    public TaxBandService(ILogger<TaxBandService> logger, ITaxBandRepository taxBandRepository)
    {
        _logger = logger;
        _taxBandRepository = taxBandRepository;
    }

    public async Task<ErrorOr<TaxBand>> CreateTaxBandAsync(TaxBand entity, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var taxBands = await _taxBandRepository.GetSortedTaxBandsAsync(cancellationToken);
        var hasNullSalaryRange = HasMultipleBandsWithUpperSalaryAsNull(taxBands, entity);

        if (HasBandWithNull(taxBands))
        {
            var error = Errors.TaxBand.BandWithInfiniteAmmountExists;

            _logger.LogError("Error while creating tax band: {Message}", error.Description);
            errors.Add(error);
            return errors;
        }

        if (hasNullSalaryRange)
        {
            var error = Errors.TaxBand.BandWithInfiniteAmmountExists;

            _logger.LogError("Error while creating tax band: {Message}", error.Description);
            errors.Add(error);
        }

        if (HasGapBetweenTaxBands(taxBands, entity) && !hasNullSalaryRange)
        {
            var error = Errors.TaxBand.GapBetweenTaxBands;

            _logger.LogError("Error while creating tax band: {Message}", error.Description);
            errors.Add(error);
        }

        if (HasOverlappingSalaryRanges(taxBands, entity) && !hasNullSalaryRange)
        {
            var error = Errors.TaxBand.OverlappingTaxBands;

            _logger.LogError("Error while creating tax band: {Message}", error.Description);
            errors.Add(error);
        }

        if (errors.Any())
        {
            return errors;
        }
        else
        {
            var dbEntity = await _taxBandRepository.InsertAsync(entity, cancellationToken);

            return dbEntity;
        }
    }

    private bool HasOverlappingSalaryRanges(IEnumerable<TaxBand> taxBands, TaxBand taxBandToAdd)
    {
        if (!taxBands.Any())
        {
            return false;
        }

        // x1<=y2 && y1<x2: https://stackoverflow.com/questions/3269434/whats-the-most-efficient-way-to-test-if-two-ranges-overlap
        // a bit changed because the taxbands start from the value of the max salary of the band that was before
        foreach (var taxBand in taxBands)
        {
            if (taxBand.LowerSalaryRange <= taxBandToAdd.UpperSalaryRange && taxBandToAdd.LowerSalaryRange < taxBand.UpperSalaryRange)
            {
                return true;
            }
        }

        return false;
    }

    private bool HasGapBetweenTaxBands(IEnumerable<TaxBand> taxBands, TaxBand taxBandToAdd)
    {
        var taxBandsWithNewBand = taxBands.ToList();
        taxBandsWithNewBand.Add(taxBandToAdd);
        taxBandsWithNewBand = taxBandsWithNewBand.OrderBy(x => x.LowerSalaryRange).ToList();

        if (taxBands.Count() == 0)
        {
            return false;
        }

        for (int i = 1; i < taxBandsWithNewBand.Count(); i++)
        {
            if (taxBands.ElementAt(i - 1).UpperSalaryRange != taxBandsWithNewBand.ElementAt(i).LowerSalaryRange)
            {
                return true;
            }
        }

        return false;
    }

    private bool HasMultipleBandsWithUpperSalaryAsNull(IEnumerable<TaxBand> taxBands, TaxBand taxBandToAdd)
    {
        var taxBandWithNullExists = taxBands.Any(x => x.UpperSalaryRange == null);

        if (taxBandToAdd.UpperSalaryRange == null && taxBandWithNullExists == true)
        {
            return true;
        }

        return false;
    }

    private bool HasBandWithNull(IEnumerable<TaxBand> taxBands)
    {
        return taxBands.Any(x => x.UpperSalaryRange == null);
    }
}