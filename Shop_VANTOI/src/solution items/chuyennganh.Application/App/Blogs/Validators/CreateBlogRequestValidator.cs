using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.App.Blogs.Handler;
using FluentValidation;

namespace chuyennganh.Application.App.Blogs.Validators
{
    public class CreateBlogRequestValidator : AbstractValidator<CreateBlogRequest>
    {
        public CreateBlogRequestValidator() { }
    }
}
