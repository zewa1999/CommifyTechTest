using Microsoft.AspNetCore.Mvc;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Contracts.Request;

namespace Tax.Calculator.Commify.Controllers;

[ApiController]
[Route("api/tax/calculator/v1")]
public class TaxCalculatorController : ApiController
{
    private readonly ILogger<TaxCalculatorController> _logger;
    private readonly ITaxCalculatorService _taxCalculatorService;

    public TaxCalculatorController(ITaxCalculatorService taxCalculatorService, ILogger<TaxCalculatorController> logger)
    {
        _taxCalculatorService = taxCalculatorService;
        _logger = logger;
    }

    [HttpPost]
    [Route("calculate")]
    public async Task<IActionResult> CalculateTaxAsync(CalculateTaxRequestDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started calculating taxes with gross salary of {Salary}", request.GrossAnnualSalary);

        var taxData = await _taxCalculatorService.CalculateTaxAsync(request, cancellationToken);

        _logger.LogInformation("Taxes for the gross salary of {Salary} are: {TaxesPaid}", request.GrossAnnualSalary, taxData.AnnualTaxPaid);

        return Ok(taxData);
    }
}