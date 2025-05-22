using chuyennganh.Application.App.ProductApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.ProductApp.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");

            RuleFor(p => p.ProductName)
               .MaximumLength(200).WithMessage("ProductName không được vượt quá 200 ký tự.");

            RuleFor(p => p.RegularPrice)
                .GreaterThanOrEqualTo(0).WithMessage("RegularPrice phải lớn hơn hoặc bằng 0.");

            RuleFor(p => p.DiscountPrice)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountPrice phải lớn hơn hoặc bằng 0.");

            RuleFor(p => p.Description)
                .MaximumLength(200).WithMessage("Description không được vượt quá 200 ký tự.");

            RuleFor(p => p.Brand)
                .MaximumLength(2000).WithMessage("Brand không được vượt quá 2000 ký tự.");

            RuleFor(p => p.Size)
                .MaximumLength(200).WithMessage("Size không được vượt quá 200 ký tự.");

            RuleFor(p => p.Color)
                .MaximumLength(200).WithMessage("Color không được vượt quá 200 ký tự.");

            RuleFor(p => p.Material)
                .MaximumLength(2000).WithMessage("Material không được vượt quá 200 ký tự.");

            RuleFor(p => p.Gender)
                .MaximumLength(10).WithMessage("Gender không được vượt quá 10 ký tự.");

            RuleFor(p => p.Packaging)
                .MaximumLength(2000).WithMessage("Packaging không được vượt quá 2000 ký tự.");

            RuleFor(p => p.Origin)
                .MaximumLength(2000).WithMessage("Origin không được vượt quá 2000 ký tự.");

            RuleFor(p => p.Manufacturer)
                .MaximumLength(2000).WithMessage("Manufacturer không được vượt quá 2000 ký tự.");

            RuleFor(p => p.SeoTitle)
                .MaximumLength(200).WithMessage("SeoTitle không được vượt quá 200 ký tự.");

            RuleFor(p => p.SeoAlias)
                .MaximumLength(200).WithMessage("SeoAlias không được vượt quá 200 ký tự.");

            RuleFor(x => x.CategoryIds).Must(list => list.All(id => id > 0)).WithMessage("Category Id phải lớn hơn 0.");
        }
    }
}
