using Microsoft.AspNetCore.Mvc;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Mappers;

namespace Tax.Calculator.Commify.Controllers;

[ApiController]
[Route("api/tax/taxband/v1")]
public class TaxBandController : ApiController
{
    private readonly ILogger<TaxBandController> _logger;
    private readonly ITaxBandService _taxBandService;

    public TaxBandController(ILogger<TaxBandController> logger, ITaxBandService taxBandService)
    {
        _logger = logger;
        _taxBandService = taxBandService;
    }

    [HttpPost]
    [Route("create-taxband")]
    public async Task<IActionResult> CreateTaxBandAsync(CreateTaxBandRequestDto request, CancellationToken cancellationToken)
    {
        var mappedEntity = TaxBandMapper.MapToTaxBand(request);

        _logger.LogInformation("Creating new taxband with: {Request}", request);

        var taxBandResult = await _taxBandService.CreateTaxBandAsync(mappedEntity, cancellationToken);

        return taxBandResult.Match(
        authResult => Ok(taxBandResult.Value),
        errors => Problem(errors));
    }
}