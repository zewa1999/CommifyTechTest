namespace Tax.Calculator.Commify.Contracts.Response;

public record CalculateTaxResponseDto
{
    public uint GrossAnnualSalary { get; set; }
    public decimal GrossMonthlySalary { get; set; }
    public uint NetAnnualSalary { get; set; }
    public decimal NetMonthlySalary { get; set; }
    public decimal AnnualTaxPaid { get; set; }
    public decimal MonthlyTaxPaid { get; set; }
}