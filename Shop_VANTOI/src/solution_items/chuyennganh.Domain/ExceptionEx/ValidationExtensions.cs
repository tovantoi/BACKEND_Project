using chuyennganh.Domain.Constants;
using chuyennganh.Domain.DependencyInjection.Extensions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Validators;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Domain.ExceptionEx
{
    public static class ValidationExtensions
    {
        public static void ThrowIfInvalid(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ShopException(StatusCodes.Status400BadRequest,
                    new List<string> { validationResult.ErrorMessage });
            }
        }

        public static void ThrowNotFound<T>(this T entity, string? errorMessage = null)
        {
            if (entity is null)
            {
                var entityTypeName = typeof(T).Name;
                errorMessage = $"{entityTypeName} is not found!";
                throw new ShopException(StatusCodes.Status404NotFound, new List<string> { errorMessage });
            }
        }

        public static void ThrowConflict<T>(this T entity, string? errorMessage = null)
        {
            if (entity is not null)
            {
                var entityTypeName = typeof(T).Name;
                errorMessage = $"{entityTypeName} is conflict";
                throw new ShopException(StatusCodes.Status409Conflict, new List<string> { errorMessage });
            }
        }

        public static ValidationRule<T, string> IsBase64<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.MUST_BE_BASE_64.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value =>
            {
                try
                {
                    Convert.FromBase64String(value);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }, messageCode, errorMessage);
        }
    }
}