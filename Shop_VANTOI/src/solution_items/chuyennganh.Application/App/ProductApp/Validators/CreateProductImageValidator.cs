using chuyennganh.Application.App.ProductApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.ProductApp.Validators
{
    public class CreateProductImageValidator : AbstractValidator<CreateProductImageCommand>
    {
        public CreateProductImageValidator()
        { }
    }
}
