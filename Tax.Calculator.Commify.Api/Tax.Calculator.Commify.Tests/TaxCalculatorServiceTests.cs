using Moq;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Data.Entities;
using Tax.Calculator.Commify.Data.Repositories;

namespace Tax.Calculator.Commify.Tests;

public class TaxCalculatorServiceTests
{
    private readonly Mock<ITaxBandRepository> _taxBandRepositoryMock;
    private readonly ITaxCalculatorService _taxCalculatorService;

    public TaxCalculatorServiceTests()
    {
        _taxBandRepositoryMock = new Mock<ITaxBandRepository>();
        _taxCalculatorService = new TaxCalculatorService(_taxBandRepositoryMock.Object);
    }

    [Theory]
    [InlineData(40000, 3333.33, 29000, 2416.67, 11000.00, 916.67)]
    [InlineData(21000, 1750, 17600, 1466.67, 3400, 283.33)]
    [InlineData(70000, 5833.33, 47000, 3916.67, 23000, 1916.67)]
    [InlineData(4500, 375, 4500, 375, 0, 0)]
    public async Task CalculateTax_Should_Return_CorrectAmmountOfTax(uint expectedGrossAnnualSalary,
                                                                     decimal expectedGrossMonthlySalary,
                                                                     uint expectedNetAnnualSalary,
                                                                     decimal expectedNetMonthlySalary,
                                                                     decimal expectedAnnualTaxPaid,
                                                                     decimal expectedMonthlyTaxPaid)
    {
        var taxBands = new List<TaxBand>
        {
            new TaxBand
            {
                Id = Guid.NewGuid(),
                BandName = "Tax Band A",
                LowerSalaryRange = 0,
                UpperSalaryRange = 5000,
                TaxRate = 0
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
                BandName = "Tax Band C",
                LowerSalaryRange = 20000,
                UpperSalaryRange = null,
                TaxRate = 40
            }
        };

        _taxBandRepositoryMock.Setup(repo => repo.GetSortedTaxBandsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(taxBands);

        var actual = await _taxCalculatorService.CalculateTaxAsync(new CalculateTaxRequestDto
        {
            GrossAnnualSalary = expectedGrossAnnualSalary
        },
        CancellationToken.None);

        Assert.Equal(expectedGrossAnnualSalary, actual.GrossAnnualSalary);
        Assert.Equal(expectedGrossMonthlySalary, actual.GrossMonthlySalary);
        Assert.Equal(expectedNetAnnualSalary, actual.NetAnnualSalary);
        Assert.Equal(expectedNetMonthlySalary, actual.NetMonthlySalary);
        Assert.Equal(expectedAnnualTaxPaid, actual.AnnualTaxPaid);
        Assert.Equal(expectedMonthlyTaxPaid, actual.MonthlyTaxPaid);
    }
}