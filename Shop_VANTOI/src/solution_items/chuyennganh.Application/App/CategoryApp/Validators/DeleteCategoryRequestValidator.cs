using chuyennganh.Application.App.CategoryApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CategoryApp.Validators
{
    public class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequest>
    {
        public DeleteCategoryRequestValidator() { }
    }
}
