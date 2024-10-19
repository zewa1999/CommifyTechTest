using Microsoft.EntityFrameworkCore;
using Tax.Calculator.Commify.Data;
using Tax.Calculator.Commify.Data.Entities;
using Tax.Calculator.Commify.Data.Repositories;
using Tax.Calculator.Commify.Tests.Utils;

namespace Tax.Calculator.Commify.Tests;

public class TaxBandRepositoryTests
{
    private readonly ITaxBandRepository _taxBandRepository;
    private readonly TaxCalculatorDbContext _inMemoryContext;

    public TaxBandRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TaxCalculatorDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _inMemoryContext = new TaxCalculatorDbContext(options);

        _taxBandRepository = new TaxBandRepository(_inMemoryContext);
    }

    [Fact]
    public async Task GetSortedTaxBandsAsync_Should_Return_SortedData()
    {
        var taxBands = TestUtils.TaxBandsData.ToList();
        await _inMemoryContext.TaxBands.AddRangeAsync(taxBands);
        await _inMemoryContext.SaveChangesAsync();

        var result = await _taxBandRepository.GetSortedTaxBandsAsync(CancellationToken.None);

        var expected = taxBands.OrderBy(x => x.LowerSalaryRange).ToList();
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetSortedTaxBandsAsync_Should_Return_EmptyList_If_DbEmpty()
    {
        var result = await _taxBandRepository.GetSortedTaxBandsAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task InsertAsync_Should_InsertData()
    {
        var taxBand = new TaxBand
        {
            Id = Guid.NewGuid(),
            BandName = "Tax Band D",
            LowerSalaryRange = 20000,
            UpperSalaryRange = null,
            TaxRate = 40
        };

        await _taxBandRepository.InsertAsync(taxBand, CancellationToken.None);

        var result = await _inMemoryContext.TaxBands.FindAsync(taxBand.Id);
        Assert.NotNull(result);
        Assert.Equal(taxBand.BandName, result.BandName);
        Assert.Equal(taxBand.LowerSalaryRange, result.LowerSalaryRange);
        Assert.Equal(taxBand.UpperSalaryRange, result.UpperSalaryRange);
        Assert.Equal(taxBand.TaxRate, result.TaxRate);
    }
}