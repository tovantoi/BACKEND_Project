namespace chuyennganh.Domain.Constants
{
    /// <summary>
    /// Contain regex string constant for application
    /// </summary>
    public class RegexConst
    {
        public const string CONST_CASE_WITH_SPECIAL = @"^[A-Z0-9@#\$%\^&\*\-!]+(_[A-Z0-9@#\$%\^&\*\-!]+)*$";
        public const string CONST_CASE = @"^[A-Z0-9]+(_[A-Z0-9]+)*$";
        public const string KEBAB_CASE_WITH_SPECIAL = @"^[a-z0-9@#\$%\^&\*\!]+(-[a-z0-9@#\$%\^&\*\!]+)*$";
        public const string KEBAB_CASE = @"^[a-z0-9]+(?:-[a-z0-9]+)*$";
        public const string NOT_VIETNAMESE = @"^[\x00-\x7F]*$";
        public const string EMAIL = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public const string PHONE_NUMBER = @"^\+?\d{10,15}$";
    }
}