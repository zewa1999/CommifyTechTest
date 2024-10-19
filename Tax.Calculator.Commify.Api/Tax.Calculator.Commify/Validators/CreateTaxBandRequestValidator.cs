using FluentValidation;
using Tax.Calculator.Commify.Contracts.Request;

namespace Tax.Calculator.Commify.Validators;

public class CreateTaxBandRequestValidator : AbstractValidator<CreateTaxBandRequestDto>
{
    public CreateTaxBandRequestValidator()
    {
        RuleFor(t => t.BandName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(1, 50).WithMessage("{PropertyName} must be between 2 and 50 characters.");

        RuleFor(t => t.LowerSalaryRange)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal to 0.");

        RuleFor(t => t.UpperSalaryRange)
            .GreaterThan(t => t.LowerSalaryRange)
            .When(t => t.UpperSalaryRange.HasValue)
            .WithMessage("{PropertyName} must be greater than LowerSalaryRange.");

        RuleFor(t => t.TaxRate)
            .InclusiveBetween(0, 100).WithMessage("{PropertyName} must be between 0 and 100.");
    }
}