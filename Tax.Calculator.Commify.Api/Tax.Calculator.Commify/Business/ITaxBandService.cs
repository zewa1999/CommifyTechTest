using ErrorOr;
using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Business;

public interface ITaxBandService
{
    public Task<ErrorOr<TaxBand>> CreateTaxBandAsync(TaxBand taxBand, CancellationToken cancellationToken);
}