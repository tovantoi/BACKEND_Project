using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Enumerations
{
    /// <summary>
    /// Enum to define error code, use for decompile into message for end user
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MsgCode
    {
        #region FileSERVER msg codes

        /// <summary>
        /// File size exceed limit
        /// </summary>
        ERR_FILE_TOO_LARGE,

        /// <summary>
        /// File extension is not permitted
        /// </summary>
        ERR_INVALID_FILE,
        #endregion

        ERR_UNSUPPORTED_MEDIA_TYPE,

        ERR_BAD_REQUEST,

        #region Base msg codes

        /// <summary>
        /// Define error code for invalid email format
        /// </summary>
        /// <remarks>
        /// Correct example: abc@gmail.com
        /// </remarks>
        ERR_INVALID_EMAIL,

        /// <summary>
        /// Define error code for invalid phone format
        /// </summary>
        /// <remarks>
        /// Correct example: +84 123 123 1234
        /// </remarks>
        ERR_INVALID_PHONE,

        /// <summary>
        /// Define error for invalid key format
        /// </summary>
        /// <remarks>
        /// Correct example: ABC_123
        /// </remarks>
        ERR_INVALID_KEY,

        /// <summary>
        /// Define error code for internal server error
        /// </summary>
        ERR_INTERNAL_SERVER,

        /// <summary>
        /// Define error code for not found resources
        /// </summary>
        ERR_NOT_FOUND,

        /// <summary>
        /// Define error code for conflict between resources
        /// </summary>
        ERR_CONFLICT,

        /// <summary>
        /// Define error code for unexpected validation exception
        /// </summary>
        ERR_INVALID,

        /// <summary>
        /// Define error code for not found resources find by key
        /// </summary>
        ERR_NF_FIND_KEY,

        /// <summary>
        /// Define code for created message
        /// </summary>
        INF_CREATED,

        /// <summary>
        /// Define code for updated message
        /// </summary>
        INF_UPDATED,

        /// <summary>
        /// Define code for deleted message
        /// </summary>
        INF_DELETED,

        /// <summary>
        /// Define code for found resource
        /// </summary>
        INF_FOUND,
        #endregion
    }
}
