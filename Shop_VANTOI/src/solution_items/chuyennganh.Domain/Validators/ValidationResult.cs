using chuyennganh.Domain.Enumerations;

namespace chuyennganh.Domain.Validators
{
    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Indicate valid state
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Error messages if not valid
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Message code
        /// </summary>
        public MsgCode MessageCode { get; set; }

        /// <summary>
        /// Helper to create success result
        /// </summary>
        public static ValidationResult Success => new() { IsValid = true };

        /// <summary>
        /// Helper to create failed result
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ValidationResult Fail(MsgCode messageCode, string errorMessage)
        {
            return new ValidationResult
            {
                IsValid = false,
                MessageCode = messageCode,
                ErrorMessage = errorMessage
            };
        }
    }
}