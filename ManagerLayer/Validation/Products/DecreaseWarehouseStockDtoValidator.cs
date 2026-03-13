using EntityLayer.Dtos.Products;
using FluentValidation;

namespace ManagerLayer.Validation.Products;

public class DecreaseWarehouseStockDtoValidator : AbstractValidator<DecreaseWarehouseStockDto>
{
    public DecreaseWarehouseStockDtoValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("WarehouseId zorunludur.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity 0'dan büyük olmalýdýr.");
    }
}
