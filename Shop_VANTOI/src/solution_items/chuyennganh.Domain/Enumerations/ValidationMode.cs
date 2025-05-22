namespace chuyennganh.Domain.Enumerations
{
    /// <summary>
    /// Mode to validate
    /// </summary>
    public enum ValidationMode
    {
        /// <summary>
        /// Validate all sentence, return result after done
        /// </summary>
        ALL,

        /// <summary>
        /// Stop when one sentence is failed
        /// </summary>
        SINGLE
    }
}