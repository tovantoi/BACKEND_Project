using chuyennganh.Application.App.CouponApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CouponApp.Validators
{
    public class UpdateCouponRequestValidator : AbstractValidator<CouponUpdateRequest>
    {
        public UpdateCouponRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("Id không được để trống.")
                .NotEmpty().WithMessage("Id không được để rỗng.")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");

            RuleFor(c => c.Code)
                .MaximumLength(2000).WithMessage("Code không được vượt quá 2000 ký tự.");

            RuleFor(c => c.Description)
                .MaximumLength(200).WithMessage("Description không được vượt quá 200 ký tự.");

            RuleFor(c => c.TimesUsed)
                .GreaterThanOrEqualTo(0).WithMessage("TimesUsed phải lớn hơn hoặc bằng 0.");

            RuleFor(c => c.MaxUsage)
                .GreaterThanOrEqualTo(0).WithMessage("MaxUsage phải lớn hơn hoặc bằng 0.");

            RuleFor(c => c.Discount)
                .MaximumLength(2000).WithMessage("Discount không được vượt quá 2000 ký tự.");
        }
    }
}
