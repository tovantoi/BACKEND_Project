using chuyennganh.Domain.Constants;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using System.Linq.Expressions;

namespace chuyennganh.Domain.Validators
{
    /// <summary>
    /// Validator class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Validator<T>
    {
        private readonly List<IValidationRule<T>> rules;
        private MsgCode msgCode;
        private ValidationMode mode;
        private string msg;

        protected Validator()
        {
            rules = new List<IValidationRule<T>>();
            msgCode = MsgCode.ERR_INVALID;
            mode = ValidationMode.ALL;
            msg = MsgConst.INVALID_DEFAULT;
        }

        /// <summary>
        /// Set up rule for field, property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        protected ValidationRule<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, ValidationMode mode = ValidationMode.ALL)
        {
            ValidationRule<T, TProperty> rule = new(propertyExpression, mode, msgCode, msg);
            rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Init validator with custom params
        /// </summary>
        /// <param name="msgCode"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Validator<T> WithValidator(MsgCode msgCode, string? msg = null, ValidationMode? mode = null)
        {
            this.msgCode = msgCode;
            this.mode = mode ?? this.mode;
            this.msg = msg ?? this.msg;
            return this;
        }

        /// <summary>
        /// Set validator mode
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Validator<T> Mode(ValidationMode mode)
        {
            this.mode = mode;
            return this;
        }

        /// <summary>
        /// Validate instance to get validation results base on rules
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public List<ValidationResult> Validate(T instance)
        {
            List<ValidationResult> results = new();
            foreach (IValidationRule<T> rule in rules)
            {
                List<ValidationResult> result = rule.Validate(instance);
                if (result.Any())
                {
                    results.AddRange(result);
                    if (mode == ValidationMode.SINGLE || rule.Mode == ValidationMode.SINGLE)
                        break;
                }
            }

            return results;
        }

        /// <summary>
        /// Validate instance base on rules and throw <see cref="CustomException"/> if rule is not match
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="CustomException"></exception>
        public void ValidateAndThrow(T instance)
        {
            List<ValidationResult> allErrors = Validate(instance);
            if (allErrors.Any())
            {
                MsgCode singleCode = mode == ValidationMode.SINGLE ? allErrors.FirstOrDefault().MessageCode : msgCode;
                ShopException.ThrowValidationException(singleCode, allErrors.Select(w => w.ErrorMessage).ToArray());
            }
        }

        /// <summary>
        /// Check instance is valid base on rules
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool IsValid(T instance)
        {
            List<ValidationResult> validationResults = Validate(instance);
            return !validationResults.Any();
        }
    }
}