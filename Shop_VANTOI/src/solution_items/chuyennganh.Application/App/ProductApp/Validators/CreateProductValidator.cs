using chuyennganh.Application.App.ProductApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.ProductApp.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.ProductName)
               .NotNull().WithMessage("ProductName không được để trống.")
               .NotEmpty().WithMessage("ProductName không được rỗng.")
               .MaximumLength(200).WithMessage("ProductName không được vượt quá 200 ký tự.");

            RuleFor(p => p.RegularPrice)
                .GreaterThanOrEqualTo(0).WithMessage("RegularPrice phải lớn hơn hoặc bằng 0.");

            RuleFor(p => p.DiscountPrice)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountPrice phải lớn hơn hoặc bằng 0.");

            RuleFor(p => p.Description)
                .NotNull().WithMessage("Description không được để trống.")
                .NotEmpty().WithMessage("Description không được rỗng.")
                .MaximumLength(200).WithMessage("Description không được vượt quá 200 ký tự.");

            RuleFor(p => p.Brand)
                .NotNull().WithMessage("Brand không được để trống.")
                .NotEmpty().WithMessage("Brand không được rỗng.")
                .MaximumLength(2000).WithMessage("Brand không được vượt quá 2000 ký tự.");

            RuleFor(p => p.Size)
                .NotNull().WithMessage("Size không được để trống.")
                .NotEmpty().WithMessage("Size không được rỗng.")
                .MaximumLength(200).WithMessage("Size không được vượt quá 200 ký tự.");

            RuleFor(p => p.Color)
                .NotNull().WithMessage("Color không được để trống.")
                .NotEmpty().WithMessage("Color không được rỗng.")
                .MaximumLength(200).WithMessage("Color không được vượt quá 200 ký tự.");

            RuleFor(p => p.Material)
                .NotNull().WithMessage("Material không được để trống.")
                .NotEmpty().WithMessage("Material không được rỗng.")
                .MaximumLength(2000).WithMessage("Material không được vượt quá 200 ký tự.");

            RuleFor(p => p.Gender)
                .NotNull().WithMessage("Gender không được để trống.")
                .NotEmpty().WithMessage("Gender không được rỗng.")
                .MaximumLength(10).WithMessage("Gender không được vượt quá 10 ký tự.");

            RuleFor(p => p.Packaging)
                .NotNull().WithMessage("Packaging không được để trống.")
                .NotEmpty().WithMessage("Packaging không được rỗng.")
                .MaximumLength(2000).WithMessage("Packaging không được vượt quá 2000 ký tự.");

            RuleFor(p => p.Origin)
                .NotNull().WithMessage("Origin không được để trống.")
                .NotEmpty().WithMessage("Origin không được rỗng.")
                .MaximumLength(2000).WithMessage("Origin không được vượt quá 2000 ký tự.");

            RuleFor(p => p.Manufacturer)
                .NotNull().WithMessage("Manufacturer không được để trống.")
                .NotEmpty().WithMessage("Manufacturer không được rỗng.")
                .MaximumLength(2000).WithMessage("Manufacturer không được vượt quá 2000 ký tự.");

            RuleFor(p => p.SeoTitle)
                .NotNull().WithMessage("SeoTitle không được để trống.")
                .NotEmpty().WithMessage("SeoTitle không được rỗng.")
                .MaximumLength(200).WithMessage("SeoTitle không được vượt quá 200 ký tự.");

            RuleFor(p => p.SeoAlias)
                .NotNull().WithMessage("SeoAlias không được để trống.")
                .NotEmpty().WithMessage("SeoAlias không được rỗng.")
                .MaximumLength(200).WithMessage("SeoAlias không được vượt quá 200 ký tự.");

            RuleFor(p => p.IsActive)
                .NotEmpty().WithMessage("Description không được rỗng.")
                .NotNull().WithMessage("IsActive không được để trống.");

            RuleFor(x => x.CategoryIds).Must(list => list.All(id => id > 0)).WithMessage("Category Id phải lớn hơn 0.");
        }
    }
}
