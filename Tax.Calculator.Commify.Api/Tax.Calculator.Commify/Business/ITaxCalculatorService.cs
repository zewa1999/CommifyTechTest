using Tax.Calculator.Commify.Contracts.Request;
using Tax.Calculator.Commify.Contracts.Response;

namespace Tax.Calculator.Commify.Business;

public interface ITaxCalculatorService
{
    Task<CalculateTaxResponseDto> CalculateTaxAsync(CalculateTaxRequestDto request, CancellationToken cancellationToken);
}