using chuyennganh.Application.App.CouponApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CouponApp.Validators
{
    public class CreateCouponRequestValidator : AbstractValidator<CouponCreateRequest>
    {
        public CreateCouponRequestValidator()
        {
            RuleFor(c => c.Code)
                .NotNull().WithMessage("Code không được để trống.")
                .NotEmpty().WithMessage("Code không được rỗng.")
                .MaximumLength(2000).WithMessage("Code không được vượt quá 2000 ký tự.");

            RuleFor(c => c.Description)
                .MaximumLength(200).WithMessage("Mô tả không được vượt quá 200 ký tự.");

            RuleFor(c => c.TimesUsed)
                .NotNull().WithMessage("TimesUsed không được để trống.")
                .NotEmpty().WithMessage("TimesUsed không được rỗng.")
                .GreaterThanOrEqualTo(0).WithMessage("TimesUsed phải lớn hơn hoặc bằng 0.");

            RuleFor(c => c.MaxUsage)
                .NotNull().WithMessage("MaxUsage không được để trống.")
                .NotEmpty().WithMessage("MaxUsage không được rỗng.")
                .GreaterThanOrEqualTo(0).WithMessage("MaxUsage phải lớn hơn hoặc bằng 0.");

            RuleFor(c => c.Discount)
                .NotNull().WithMessage("Discount không được để trống.")
                .NotEmpty().WithMessage("Discount không được rỗng.")
                .MaximumLength(2000).WithMessage("Discount không được vượt quá 2000 ký tự.");

            RuleFor(c => c.IsActive)
                .NotEmpty().WithMessage("IsActive không được rỗng.")
                .NotNull().WithMessage("IsActive không được để trống.");
        }
    }
}
