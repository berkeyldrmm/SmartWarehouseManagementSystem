using EntityLayer.Dtos.Companies.Requests;
using FluentValidation;

namespace ManagerLayer.Validation.Companies;

public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Þirket adý zorunludur.")
            .MaximumLength(200).WithMessage("Þirket adý en fazla 200 karakter olabilir.");
    }
}
