using Microsoft.EntityFrameworkCore;
using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Data.Repositories;

public class TaxBandRepository : ITaxBandRepository
{
    private readonly TaxCalculatorDbContext _dbContext;

    public TaxBandRepository(TaxCalculatorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TaxBand>> GetSortedTaxBandsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.TaxBands
            .OrderBy(x => x.LowerSalaryRange)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaxBand> InsertAsync(TaxBand taxBand, CancellationToken cancellationToken)
    {
        var dbEntry = await _dbContext.TaxBands
            .AddAsync(taxBand, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return dbEntry.Entity;
    }
}