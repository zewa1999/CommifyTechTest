namespace Tax.Calculator.Commify.Contracts.Request;

public record CalculateTaxRequestDto
{
    public uint GrossAnnualSalary { get; set; }
}