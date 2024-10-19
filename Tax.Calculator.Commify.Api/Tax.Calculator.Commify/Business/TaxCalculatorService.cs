using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Contracts.Response;
using Tax.Calculator.Commify.Data.Entities;
using Tax.Calculator.Commify.Data.Repositories;

namespace Tax.Calculator.Commify.Business;

public class TaxCalculatorService : ITaxCalculatorService
{
    private readonly ITaxBandRepository _taxBandRepository;

    public TaxCalculatorService(ITaxBandRepository taxBandRepository)
    {
        _taxBandRepository = taxBandRepository;
    }

    public async Task<CalculateTaxResponseDto> CalculateTaxAsync(CalculateTaxRequestDto request, CancellationToken cancellationToken)
    {
        var taxBands = await _taxBandRepository.GetSortedTaxBandsAsync(cancellationToken);
        decimal annualTaxPaid;

        if (taxBands == null || !taxBands.Any())
        {
            annualTaxPaid = 0;
        }
        else
        {
            annualTaxPaid = CalculateAnnualTax(request.GrossAnnualSalary, taxBands);
        }

        var netAnnualSalary = CalculateNetAnnualSalary(request.GrossAnnualSalary, annualTaxPaid);
        var netMonthlySalary = CalculateNetMonthlySalary(netAnnualSalary);

        return new CalculateTaxResponseDto
        {
            GrossAnnualSalary = request.GrossAnnualSalary,
            GrossMonthlySalary = CalculateGrossMonthlySalary(request.GrossAnnualSalary),
            NetAnnualSalary = netAnnualSalary,
            NetMonthlySalary = netMonthlySalary,
            AnnualTaxPaid = annualTaxPaid,
            MonthlyTaxPaid = CalculateMonthlyTaxPaid(annualTaxPaid)
        };
    }

    private decimal CalculateMonthlyTaxPaid(decimal totalTaxPaid)
    {
        var result = (decimal)totalTaxPaid / 12;
        return Math.Round(result, 2);
    }

    private decimal CalculateNetMonthlySalary(uint netAnnualSalary)
    {
        var result = (decimal)netAnnualSalary / 12;
        return Math.Round(result, 2);
    }

    private uint CalculateNetAnnualSalary(uint grossAnnualSalary, decimal totalTaxPaid)
    {
        return grossAnnualSalary - (uint)totalTaxPaid;
    }

    private decimal CalculateGrossMonthlySalary(uint grossAnnualSalary)
    {
        var result = (decimal)grossAnnualSalary / 12;
        return Math.Round(result, 2);
    }

    private decimal CalculateAnnualTax(uint grossAnnualSalary, IEnumerable<TaxBand> taxBands)
    {
        decimal annualTaxPaid = 0M;

        foreach (var taxBand in taxBands)
        {
            if (grossAnnualSalary <= taxBand.LowerSalaryRange)
            {
                break;
            }

            decimal bandSalary = 0M;

            if (taxBand.UpperSalaryRange != null)
            {
                decimal min = Math.Min(grossAnnualSalary, taxBand.UpperSalaryRange.Value);
                bandSalary = min - taxBand.LowerSalaryRange;
            }
            else
            {
                bandSalary = grossAnnualSalary - taxBand.LowerSalaryRange;
            }

            if (bandSalary > 0)
            {
                decimal bandTax = bandSalary * taxBand.TaxRate / 100;
                annualTaxPaid += bandTax;
            }
        }

        return Math.Round(annualTaxPaid, 2);
    }
}