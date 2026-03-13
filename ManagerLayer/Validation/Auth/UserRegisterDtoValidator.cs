using EntityLayer.Dtos.Auth;
using FluentValidation;

namespace ManagerLayer.Validation.Auth;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad zorunludur.")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Soyad zorunludur.")
            .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Kullanýcý adý zorunludur.")
            .MinimumLength(3).WithMessage("Kullanýcý adý en az 3 karakter olmalýdýr.")
            .MaximumLength(50).WithMessage("Kullanýcý adý en fazla 50 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Þifre zorunludur.")
            .MinimumLength(6).WithMessage("Þifre en az 6 karakter olmalýdýr.");

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("CompanyId zorunludur.");
    }
}
