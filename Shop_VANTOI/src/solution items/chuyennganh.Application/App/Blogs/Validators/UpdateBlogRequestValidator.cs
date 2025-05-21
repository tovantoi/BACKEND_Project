using chuyennganh.Application.App.Blogs.Command;
using FluentValidation;

namespace chuyennganh.Application.App.Blogs.Validators
{
    public class UpdateBlogRequestValidator : AbstractValidator<UpdateBlogRequest>
    {
        public UpdateBlogRequestValidator() { }
    }
}
