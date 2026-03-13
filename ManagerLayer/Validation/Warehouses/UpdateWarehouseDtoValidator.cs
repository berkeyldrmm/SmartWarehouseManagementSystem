using EntityLayer.Dtos.Warehouses.Requests;
using FluentValidation;

namespace ManagerLayer.Validation.Warehouses;

public class UpdateWarehouseDtoValidator : AbstractValidator<UpdateWarehouseDto>
{
    public UpdateWarehouseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Depo adý zorunludur.")
            .MaximumLength(150).WithMessage("Depo adý en fazla 150 karakter olabilir.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Depo konumu zorunludur.")
            .MaximumLength(300).WithMessage("Depo konumu en fazla 300 karakter olabilir.");
    }
}
