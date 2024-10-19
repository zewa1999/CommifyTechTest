using Microsoft.Extensions.Logging;
using Moq;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Data.Entities;
using Tax.Calculator.Commify.Data.Repositories;
using Tax.Calculator.Commify.Tests.Utils;

namespace Tax.Calculator.Commify.Tests;

public class TaxBandServiceTests
{
    private readonly Mock<ITaxBandRepository> _taxBandRepositoryMock;
    private readonly ITaxBandService _taxBandService;
    private readonly Mock<ILogger<TaxBandService>> _loggerMock;

    public TaxBandServiceTests()
    {
        _taxBandRepositoryMock = new Mock<ITaxBandRepository>();
        _loggerMock = new Mock<ILogger<TaxBandService>>();
        _taxBandService = new TaxBandService(_loggerMock.Object, _taxBandRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateTaxBandAsync_ShouldReturnError_WhenBandWithNullUpperSalaryExists()
    {
        var existingTaxBands = new List<TaxBand>
        {
            TestUtils.CreateTaxBand(20000, null)
        };

        _taxBandRepositoryMock.Setup(repo => repo.GetSortedTaxBandsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTaxBands);

        var newTaxBand = TestUtils.CreateTaxBand(30000, null);

        var result = await _taxBandService.CreateTaxBandAsync(newTaxBand, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == Data.Errors.Errors.TaxBand.BandWithInfiniteAmmountExists.Code);
    }

    [Fact]
    public async Task CreateTaxBandAsync_ShouldReturnError_WhenGapExistsBetweenTaxBands()
    {
        var existingTaxBands = new List<TaxBand>
        {
            TestUtils.CreateTaxBand(0, 5000),
            TestUtils.CreateTaxBand(5000, 20000)
        };

        _taxBandRepositoryMock.Setup(repo => repo.GetSortedTaxBandsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTaxBands);

        var newTaxBand = TestUtils.CreateTaxBand(25000, 30000);

        var result = await _taxBandService.CreateTaxBandAsync(newTaxBand, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == Data.Errors.Errors.TaxBand.GapBetweenTaxBands.Code);
    }

    [Fact]
    public async Task CreateTaxBandAsync_ShouldReturnError_WhenTaxBandOverlaps()
    {
        var existingTaxBands = new List<TaxBand>
        {
            TestUtils.CreateTaxBand(0, 5000),
            TestUtils.CreateTaxBand(5000, 20000)
        };

        _taxBandRepositoryMock.Setup(repo => repo.GetSortedTaxBandsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTaxBands);

        var newTaxBand = TestUtils.CreateTaxBand(10000, 15000);

        var result = await _taxBandService.CreateTaxBandAsync(newTaxBand, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == Data.Errors.Errors.TaxBand.OverlappingTaxBands.Code);
    }

    [Fact]
    public async Task CreateTaxBandAsync_ShouldCreateTaxBandSuccessfully_WhenNoErrors()
    {
        var existingTaxBands = new List<TaxBand>
        {
            TestUtils.CreateTaxBand(0, 5000),
            TestUtils.CreateTaxBand(5000, 20000)
        };

        _taxBandRepositoryMock.Setup(repo => repo.GetSortedTaxBandsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTaxBands);

        var newTaxBand = TestUtils.CreateTaxBand(20000, 30000);

        _taxBandRepositoryMock.Setup(repo => repo.InsertAsync(newTaxBand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newTaxBand);

        var result = await _taxBandService.CreateTaxBandAsync(newTaxBand, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(newTaxBand, result.Value);
    }

    [Fact]
    public async Task CreateTaxBandAsync_ShouldCreateFirstTaxBand_WhenNoTaxBandsExist()
    {
        var existingTaxBands = new List<TaxBand>();

        _taxBandRepositoryMock.Setup(repo => repo.GetSortedTaxBandsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTaxBands);

        var newTaxBand = TestUtils.CreateTaxBand(0, 5000);

        _taxBandRepositoryMock.Setup(repo => repo.InsertAsync(newTaxBand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newTaxBand);

        var result = await _taxBandService.CreateTaxBandAsync(newTaxBand, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(newTaxBand, result.Value);
    }
}