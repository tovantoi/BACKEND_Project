namespace chuyennganh.Domain.Constants
{
    /// <summary>
    /// Contain all constant for message in application
    /// </summary>
    public class MsgConst
    {
        public const string CONNECT_FAILED = "Can't connect to {0}";
        public const string EXT_NOT_PERMITTED = "File extensions is not permitted";
        public const string INVALID_IP_FORMAT = "{0} is invalid IP format";
        public const string CONSTANT_CASE = "{0} must be match constant case format";
        public const string KEBAB_CASE = "{0} must be match kebab case format";
        public const string NOT_CONTAIN_VIETNAMESE = "{0} must not contains vietnamese characters";
        public const string MUST_BE_BASE_64 = "{0} must be base64 format";
        public const string NOT_NULL = "{0} cannot be null.";
        public const string NOT_EMPTY = "{0} cannot be empty.";
        public const string GREATER_THAN = "{0} must be greater than {1}.";
        public const string LESS_THAN = "{0} must be less than {1}.";
        public const string GREATER_THAN_OR_EQUAL = "{0} must be greater than or equal {1}.";
        public const string LESS_THAN_OR_EQUAL = "{0} must be greater than or equal {1}.";
        public const string EQUAL = "{0} must be equal to {1}.";
        public const string NOT_EQUAL = "{0} must not be equal to {1}.";
        public const string MUST_BE_ONE_OF = "{0} must be one of the following values: {1}.";
        public const string REGEX_MISMATCH = "{0} must match the required pattern.";
        public const string REGEX_NOT_MATCH = "{0} must not match the required pattern.";
        public const string INVALID_DATE_FORMAT = "{0} must be in the format {1}.";
        public const string CONDITION_FAILED = "{0} not match condition";
        public const string INVALID_DEFAULT = "Invalid validation error please check input information again";
        public const string INVALID_EMAIL_FORMAT = "{0} is invalid email format";
        public const string NOT_EXCEED_CHAR = "{0} must not be exceed {1} characters";
        public const string NOT_LESS_THAN_CHAR = "{0} must not be less than {1} characters";
        public const string NOT_EXCEED_ELE = "{0} must not be exceed {1} elements";
        public const string NOT_LESS_THAN_ELE = "{0} must not be less than {1} elements";
        public const string LENGTH_IN_RANGE = "{0} must have length from {1} to {2}";
        public const string IN_RANGE = "{0} must be in range from {1} to {2}";
        public const string MUST_CONTAINS = "{0} must contains {1}";
        public const string MUST_START_WITH = "{0} must be started with {1}";
        public const string MUST_END_WITH = "{0} must be ended with {1}";
        public const string BE_NUMERIC = "{0} must be numeric";
        public const string INVALID_PHONE_FORMAT = "{0} is invalid phone format";
        public const string ASSET_SERVER = "asset server";
        public const string CHARACTERS = " characters";
        public const string ELEMENTS = " elements";
        public const string ENTITY = " Entity";
        public const string NOT_FOUND_FIND_KEY = "{0} was not found find by key.";
        public const string UN_SUP_MAP = "Can't map {0} because: Can't cast {1} to {2}";
    }
}