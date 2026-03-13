using EntityLayer.Dtos.Products;
using FluentValidation;

namespace ManagerLayer.Validation.Products;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün adý zorunludur.")
            .MaximumLength(100).WithMessage("Ürün adý en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Ürün açýklamasý zorunludur.")
            .MaximumLength(500).WithMessage("Ürün açýklamasý en fazla 500 karakter olabilir.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Ürün fiyatý 0'dan büyük olmalýdýr.");
    }
}
