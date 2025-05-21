using chuyennganh.Domain.Constants;
using chuyennganh.Domain.DependencyInjection.Extensions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Validators;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace chuyennganh.Domain.DependencyInjection.Extensions
{
    public static class ValidationExtensions
    {
        #region string validation

        /// <summary>
        /// Check string is in valid IPv4 format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsIPv4<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.INVALID_IP_FORMAT.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(CheckIpV4, messageCode, errorMessage);
        }

        /// <summary>
        /// Check string is in valid IPv6 format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsIPv6<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.INVALID_IP_FORMAT.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(CheckIpV6, messageCode, errorMessage);
        }

        /// <summary>
        /// Marked string must not be empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> NotEmpty<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.NOT_EMPTY.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value => !string.IsNullOrWhiteSpace(value), messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be matched constant case, Ex: ABC_XYZ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="specialChar"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsConstantCase<T>(this ValidationRule<T, string> validationRule, bool specialChar = false, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.CONSTANT_CASE.FormatMsg(validationRule.PropertyName);
            string regex = specialChar ? RegexConst.CONST_CASE_WITH_SPECIAL : RegexConst.CONST_CASE;
            return validationRule.Matches(regex, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be matched constant case, Ex: abc-xyz
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="specialChar"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsKebabCase<T>(this ValidationRule<T, string> validationRule, bool specialChar = false, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.KEBAB_CASE.FormatMsg(validationRule.PropertyName);
            string regex = specialChar ? RegexConst.KEBAB_CASE_WITH_SPECIAL : RegexConst.KEBAB_CASE;
            return validationRule.Matches(regex, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be not contains vietnamese characters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> NotContainVietnameseChar<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.NOT_CONTAIN_VIETNAMESE.FormatMsg(validationRule.PropertyName);
            return validationRule.Matches(RegexConst.NOT_VIETNAMESE, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be base64 format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Marked string must contain an element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="element"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> Contains<T>(this ValidationRule<T, string> validationRule, string element, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.MUST_CONTAINS.FormatMsg(validationRule.PropertyName, element);
            return validationRule.AddValidator(value => value.Contains(element), messageCode, errorMessage);
        }

        /// <summary>
        ///  Create rule that this property length must not exceed a specific maximum value
        /// </summary>
        /// <param name="validationRule"></param>
        /// <param name="maxLength">Max length</param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> MaxLength<T>(this ValidationRule<T, string> validationRule, int maxLength, MsgCode? messageCode = null)
        {
            string message = MsgConst.NOT_EXCEED_CHAR.FormatMsg(validationRule.PropertyName, maxLength, maxLength);
            return validationRule.AddValidator(value => value.Length <= maxLength, messageCode, message);
        }

        /// <summary>
        ///  Create rule that this property length must not less a specific minimum value
        /// </summary>
        /// <param name="validationRule"></param>
        /// <param name="minLength">Min length</param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> MinLength<T>(this ValidationRule<T, string> validationRule, int minLength, MsgCode? messageCode = null)
        {
            string message = MsgConst.NOT_LESS_THAN_CHAR.FormatMsg(validationRule.PropertyName, minLength, minLength);
            return validationRule.AddValidator(value => value.Length >= minLength, messageCode, message);
        }

        /// <summary>
        /// String length must be in range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> Length<T>(this ValidationRule<T, string> validationRule, int minLength, int maxLength, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.LENGTH_IN_RANGE.FormatMsg(validationRule.PropertyName, minLength, maxLength);
            return validationRule.AddValidator(value => value.Length >= minLength && value.Length <= maxLength, messageCode, errorMessage);
        }

        /// <summary>
        /// Create rule that this property must be valid email
        /// </summary>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsEmail<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string message = MsgConst.INVALID_EMAIL_FORMAT.FormatMsg(validationRule.PropertyName);
            // Regex check key must be alphabetical and not have any whitespace, special characters
            return validationRule.Matches(RegexConst.EMAIL, messageCode, message);
        }

        /// <summary>
        /// Create rule that this property must match a pattern
        /// </summary>
        /// <param name="validationRule"></param>
        /// <param name="regexPattern">Regex pattern</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static ValidationRule<T, string> Matches<T>(this ValidationRule<T, string> validationRule, string regexPattern, MsgCode? messageCode = null, string? message = null)
        {
            message ??= MsgConst.REGEX_MISMATCH.FormatMsg(validationRule.PropertyName);
            // Regex check key must be alphabetical and not have any whitespace, special characters
            return validationRule.AddValidator(value => Regex.Match(value, regexPattern).Success, messageCode, message);
        }

        /// <summary>
        /// Create rule that this property must not match a pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="regexPattern"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> NotMatches<T>(this ValidationRule<T, string> validationRule, string regexPattern, MsgCode? messageCode = null, string? message = null)
        {
            message ??= MsgConst.REGEX_NOT_MATCH.FormatMsg(validationRule.PropertyName);
            // Regex check key must be alphabetical and not have any whitespace, special characters
            return validationRule.AddValidator(value => !Regex.Match(value, regexPattern).Success, messageCode, message);
        }

        /// <summary>
        /// Element must be started with a prefix
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="prefix"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> StartsWith<T>(this ValidationRule<T, string> validationRule, string prefix, MsgCode? messageCode = null)
        {
            return validationRule.AddValidator(value => value.StartsWith(prefix), messageCode, MsgConst.MUST_START_WITH.FormatMsg(validationRule.PropertyName, prefix));
        }

        /// <summary>
        /// Element must be ended with a suffix
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="suffix"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> EndsWith<T>(this ValidationRule<T, string> validationRule, string suffix, MsgCode? messageCode = null)
        {
            return validationRule.AddValidator(value => value.EndsWith(suffix), messageCode, MsgConst.MUST_END_WITH.FormatMsg(validationRule.PropertyName, suffix));
        }

        /// <summary>
        /// Element must be numeric
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsNumeric<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            return validationRule.AddValidator(value => value.All(char.IsDigit), messageCode, MsgConst.BE_NUMERIC.FormatMsg(validationRule.PropertyName));
        }

        /// <summary>
        /// Element must be in phone number format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsPhoneNumber<T>(this ValidationRule<T, string> validationRule, MsgCode? messageCode = null)
        {
            string message = MsgConst.INVALID_PHONE_FORMAT.FormatMsg(validationRule.PropertyName);
            // Regex check key must be alphabetical and not have any whitespace, special characters
            return validationRule.Matches(RegexConst.PHONE_NUMBER, messageCode, message);
        }

        /// <summary>
        /// Element must be in valid date format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="format"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsValidDate<T>(this ValidationRule<T, string> validationRule, string format = DateFormats.DATETIME, MsgCode? messageCode = null)
        {
            string errorMessage = MsgConst.INVALID_DATE_FORMAT.FormatMsg(validationRule.PropertyName, format);
            return validationRule.AddValidator(value => DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _), messageCode, errorMessage);
        }

        #endregion

        #region comparable validation

        /// <summary>
        /// Element must greater than a threshold
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="threshold"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TType> GreaterThan<T, TType>(this ValidationRule<T, TType> validationRule, TType threshold, MsgCode? messageCode = null)
            where TType : struct, IComparable<TType>
        {
            return validationRule.AddValidator(value => value.CompareTo(threshold) > 0, messageCode, MsgConst.GREATER_THAN.FormatMsg(validationRule.PropertyName, threshold));
        }

        public static ValidationRule<T, TType?> GreaterThan<T, TType>(this ValidationRule<T, TType?> validationRule, TType threshold, MsgCode? messageCode = null) where TType : struct, IComparable
        {
            string errorMessage = MsgConst.GREATER_THAN.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(threshold) > 0, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must greater than or equal a threshold
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="threshold"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> GreaterThanOrEqual<T, TProperty>(this ValidationRule<T, TProperty> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : struct, IComparable<TProperty>
        {
            string errorMessage = MsgConst.GREATER_THAN_OR_EQUAL.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.CompareTo(threshold) >= 0, messageCode, errorMessage);
        }

        public static ValidationRule<T, TType?> GreaterThanOrEqual<T, TType>(this ValidationRule<T, TType?> validationRule, TType threshold, MsgCode? messageCode = null)
            where TType : struct, IComparable
        {
            string errorMessage = MsgConst.GREATER_THAN_OR_EQUAL.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(threshold) >= 0, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must less than a threshold
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="threshold"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> LessThan<T, TProperty>(this ValidationRule<T, TProperty> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : struct, IComparable<TProperty>
        {
            string errorMessage = MsgConst.LESS_THAN.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.CompareTo(threshold) < 0, messageCode, errorMessage);
        }

        public static ValidationRule<T, TType?> LessThan<T, TType>(this ValidationRule<T, TType?> validationRule, TType threshold, MsgCode? messageCode = null) where TType : struct, IComparable
        {
            string errorMessage = MsgConst.LESS_THAN.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(threshold) < 0, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must less than or equal a threshold
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="threshold"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> LessThanOrEqual<T, TProperty>(this ValidationRule<T, TProperty> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : struct, IComparable<TProperty>
        {
            string errorMessage = MsgConst.LESS_THAN_OR_EQUAL.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.CompareTo(threshold) <= 0, messageCode, errorMessage);
        }

        public static ValidationRule<T, TType?> LessThanOrEqual<T, TType>(this ValidationRule<T, TType?> validationRule, TType threshold, MsgCode? messageCode = null) where TType : struct, IComparable
        {
            string errorMessage = MsgConst.LESS_THAN_OR_EQUAL.FormatMsg(validationRule.PropertyName, threshold);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(threshold) <= 0, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be in range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> InRange<T, TProperty>(this ValidationRule<T, TProperty> validationRule, TProperty min, TProperty max, MsgCode? messageCode = null)
            where TProperty : struct, IComparable<TProperty>
        {
            string errorMessage = MsgConst.IN_RANGE.FormatMsg(validationRule.PropertyName, min, max);
            return validationRule.AddValidator(value => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0, messageCode, errorMessage);
        }

        public static ValidationRule<T, TProperty?> InRange<T, TProperty>(this ValidationRule<T, TProperty?> validationRule, TProperty min, TProperty max, MsgCode? messageCode = null)
            where TProperty : struct, IComparable
        {
            string errorMessage = MsgConst.IN_RANGE.FormatMsg(validationRule.PropertyName, min, max);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(min) >= 0 && value.Value.CompareTo(max) <= 0, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must equal a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="threshold"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> Equal<T, TProperty>(this ValidationRule<T, TProperty> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : IComparable<TProperty>
        {
            string errorMessage = MsgConst.EQUAL.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value => value.CompareTo(threshold) == 0, messageCode, errorMessage);
        }

        public static ValidationRule<T, TProperty?> Equal<T, TProperty>(this ValidationRule<T, TProperty?> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : struct, IComparable
        {
            string errorMessage = MsgConst.EQUAL.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(threshold) == 0, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must not equal a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="threshold"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> NotEqual<T, TProperty>(this ValidationRule<T, TProperty> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : IComparable<TProperty>
        {
            string errorMessage = MsgConst.NOT_EQUAL.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value => value.CompareTo(threshold) != 0, messageCode, errorMessage);
        }

        public static ValidationRule<T, TProperty?> NotEqual<T, TProperty>(this ValidationRule<T, TProperty?> validationRule, TProperty threshold, MsgCode? messageCode = null)
            where TProperty : struct, IComparable
        {
            string errorMessage = MsgConst.NOT_EQUAL.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value => value.HasValue && value.Value.CompareTo(threshold) != 0, messageCode, errorMessage);
        }

        #endregion

        #region enum, list check

        /// <summary>
        /// Element must be one of member in collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="list"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> MustBeOneOf<T, TProperty>(this ValidationRule<T, TProperty> validationRule, IEnumerable<TProperty> list, MsgCode? messageCode = null)
        {
            string validValues = string.Join(", ", list);
            string errorMessage = MsgConst.MUST_BE_ONE_OF.FormatMsg(validationRule.PropertyName, validValues);
            return validationRule.AddValidator(list.Contains, messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be a value of enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, string> IsEnumValue<T, TEnum>(this ValidationRule<T, string> validationRule, bool ignoreCase = true, MsgCode? messageCode = null) where TEnum : struct, Enum
        {
            string validValues = string.Join(", ", Enum.GetNames(typeof(TEnum)));
            string errorMessage = MsgConst.MUST_BE_ONE_OF.FormatMsg(validationRule.PropertyName, validValues);
            return validationRule.AddValidator(value => Enum.TryParse(value, ignoreCase, out TEnum result) && Enum.IsDefined(typeof(TEnum), result), messageCode, errorMessage);
        }

        /// <summary>
        /// Element must be valid in enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> IsInEnum<T, TProperty>(this ValidationRule<T, TProperty> validationRule, MsgCode? messageCode = null) where TProperty : struct, Enum
        {
            string validValues = string.Join(", ", Enum.GetNames(typeof(TProperty)));
            string errorMessage = MsgConst.MUST_BE_ONE_OF.FormatMsg(validationRule.PropertyName, validValues);
            return validationRule.AddValidator(value => Enum.IsDefined(typeof(TProperty), value), messageCode, errorMessage);
        }

        /// <summary>
        /// Element must not be empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> NotEmpty<T, TProperty>(this ValidationRule<T, TProperty> validationRule, MsgCode? messageCode = null) where TProperty : IEnumerable<object>
        {
            string errorMessage = MsgConst.NOT_EMPTY.FormatMsg(validationRule.PropertyName);
            return validationRule.AddValidator(value => value != null && value.Any(), messageCode, errorMessage);
        }

        /// <summary>
        /// Element must contains a value specific
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="element"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> Contains<T, TProperty, TElement>(this ValidationRule<T, TProperty> validationRule, TElement element, MsgCode? messageCode = null)
            where TProperty : IEnumerable<TElement>
        {
            string errorMessage = MsgConst.MUST_CONTAINS.FormatMsg(validationRule.PropertyName, element.ToString());
            return validationRule.AddValidator(value => value.Contains(element), messageCode, errorMessage);
        }

        /// <summary>
        ///  Create rule that this property length must not exceed a specific maximum value
        /// </summary>
        /// <param name="validationRule"></param>
        /// <param name="maxLength">Max length</param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> MaxLength<T, TProperty>(this ValidationRule<T, TProperty> validationRule, int maxLength, MsgCode? messageCode = null)
            where TProperty : IEnumerable<object>
        {
            string message = MsgConst.NOT_EXCEED_ELE.FormatMsg(validationRule.PropertyName, maxLength);
            return validationRule.AddValidator(value => value.Count() <= maxLength, messageCode, message);
        }

        /// <summary>
        ///  Create rule that this property length must not less a specific minimum value
        /// </summary>
        /// <param name="validationRule"></param>
        /// <param name="minLength">Min length</param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> MinLength<T, TProperty>(this ValidationRule<T, TProperty> validationRule, int minLength, MsgCode? messageCode = null)
            where TProperty : IEnumerable<object>
        {
            string message = MsgConst.NOT_LESS_THAN_ELE.FormatMsg(validationRule.PropertyName, minLength);
            return validationRule.AddValidator(value => value.Count() >= minLength, messageCode, message);
        }

        /// <summary>
        /// Element length must be in range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationRule"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static ValidationRule<T, TProperty> Length<T, TProperty>(this ValidationRule<T, TProperty> validationRule, int minLength, int maxLength, MsgCode? messageCode = null)
            where TProperty : IEnumerable<object>
        {
            string errorMessage = MsgConst.LENGTH_IN_RANGE.FormatMsg(validationRule.PropertyName, minLength, maxLength);
            return validationRule.AddValidator(value => value != null && value.Count() >= minLength && value.Count() <= maxLength, messageCode, errorMessage);
        }

        #endregion

        #region private

        private static bool CheckIpV4(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && IPAddress.TryParse(value, out IPAddress? parsedIP) && parsedIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }

        private static bool CheckIpV6(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && IPAddress.TryParse(value, out IPAddress? parsedIP) && parsedIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
        }

        #endregion
    }
}