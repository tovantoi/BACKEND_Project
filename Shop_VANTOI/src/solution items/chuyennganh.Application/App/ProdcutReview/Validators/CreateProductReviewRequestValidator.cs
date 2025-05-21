using chuyennganh.Application.App.ProdcutReview.Command;
using FluentValidation;

namespace chuyennganh.Application.App.ProdcutReview.Validators
{
    public class CreateProductReviewRequestValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateProductReviewRequestValidator() { }
    }
}
