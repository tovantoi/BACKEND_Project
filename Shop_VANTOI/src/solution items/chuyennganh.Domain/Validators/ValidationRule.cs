using chuyennganh.Domain.Constants;
using chuyennganh.Domain.DependencyInjection.Extensions;
using chuyennganh.Domain.Enumerations;
using System.Linq.Expressions;

namespace chuyennganh.Domain.Validators
{
    /// <summary>
    /// Interface for validation rule
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidationRule<T>
    {
        /// <summary>
        /// Validate rule to get result
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        List<ValidationResult> Validate(T instance);

        public ValidationMode Mode { get; }
    }

    /// <summary>
    /// Validation rule
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class ValidationRule<T, TProperty> : IValidationRule<T>
    {
        /// <summary>
        /// Validators to validate the rule
        /// </summary>
        private readonly List<Func<TProperty, ValidationResult>> validators = new();

        private readonly Func<T, TProperty> propertyFunc;
        private readonly string propertyName;
        private readonly ValidationMode mode;
        private readonly MsgCode msgCode;
        private readonly string msg;
        public ValidationMode Mode => mode;

        /// <summary>
        /// Get current property name
        /// </summary>
        public string PropertyName => propertyName;

        public ValidationRule(Expression<Func<T, TProperty>> propertyExpression, ValidationMode mode, MsgCode msgCode, string msg)
        {
            propertyFunc = propertyExpression.Compile();
            propertyName = (propertyExpression.Body as MemberExpression)?.Member.Name ?? string.Empty;
            this.mode = mode;
            this.msgCode = msgCode;
            this.msg = msg;
        }

        /// <summary>
        /// Add new validator to this rule
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="messageCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="skipNull"></param>
        /// <returns></returns>
        public ValidationRule<T, TProperty> AddValidator(Func<TProperty, bool> condition, MsgCode? messageCode = null, string? errorMessage = null)
        {
            validators.Add(value =>
            {
                if (value == null) return ValidationResult.Success;
                messageCode ??= msgCode;
                errorMessage ??= msg;
                return condition(value) ? ValidationResult.Success : ValidationResult.Fail(messageCode.Value, errorMessage);
            });
            return this;
        }

        /// <summary>
        /// Marked this entity must not be null
        /// </summary>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public ValidationRule<T, TProperty> NotNull(MsgCode? messageCode = null)
        {
            validators.Add(value => value != null ? ValidationResult.Success : ValidationResult.Fail(messageCode ?? msgCode, MsgConst.NOT_NULL.FormatMsg(propertyName)));
            return this;
        }

        /// <summary>
        /// Allow to validate entity in fluent way
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public ValidationRule<T, TProperty> Must(Func<TProperty, bool> condition, MsgCode? messageCode = null)
        {
            return AddValidator(condition, messageCode, MsgConst.CONDITION_FAILED.FormatMsg(propertyName));
        }

        /// <summary>
        /// Validate rule to get validation results
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public List<ValidationResult> Validate(T instance)
        {
            List<ValidationResult> results = new();
            TProperty value = propertyFunc(instance);
            foreach (Func<TProperty, ValidationResult> func in validators)
            {
                ValidationResult result = func(value);
                if (!result.IsValid)
                {
                    results.Add(result);
                    if (mode == ValidationMode.SINGLE)
                        break;
                }
            }

            return results;
        }
    }
}