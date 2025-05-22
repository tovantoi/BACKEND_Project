namespace chuyennganh.Domain.Errors
{
    /// <summary>
    /// Provide error for domain, contain error type and messages
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Provide details of error
        /// </summary>
        public List<string>? Details { get; }

        /// <summary>
        /// Provide stack trace causing error
        /// </summary>
        public string StackTrace { get; }

        /// <summary>
        /// Provide error for domain, contain error type and messages
        /// </summary>
        /// <param name="stackTrace"></param>
        /// <param name="details">Error messages to provide more information</param>
        public Error(string stackTrace, params string[]? details)
        {
            Details = new List<string>();

            if (details is not null)
                Details.AddRange(details);

            StackTrace = stackTrace;
        }
    }
}