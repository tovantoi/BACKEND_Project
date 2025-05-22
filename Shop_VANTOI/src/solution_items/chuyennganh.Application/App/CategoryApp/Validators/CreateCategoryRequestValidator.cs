using chuyennganh.Application.App.CategoryApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CategoryApp.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator() { }
    }
}
