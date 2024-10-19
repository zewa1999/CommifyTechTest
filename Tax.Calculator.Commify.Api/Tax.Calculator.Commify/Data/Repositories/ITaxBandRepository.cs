using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Data.Repositories;

public interface ITaxBandRepository
{
    public Task<IEnumerable<TaxBand>> GetSortedTaxBandsAsync(CancellationToken cancellationToken);

    public Task<TaxBand> InsertAsync(TaxBand taxBand, CancellationToken cancellationToken);
}