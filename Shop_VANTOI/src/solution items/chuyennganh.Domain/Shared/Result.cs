using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Errors;
using System.Net;
using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Shared
{
    /// <summary>
    /// Provide result with specific type of data for use case handle
    /// </summary>
    public class Result<TModel> where TModel : class
    {
        /// <summary>
        /// Status code of result, use same as <see cref="HttpStatusCode"/>
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Success state of result
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Data of result if any
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TModel? Data { get; set; }

        /// <summary>
        /// Data type of result if data has any
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DataType? DataType { get; set; }

        /// <summary>
        /// Message code to provide information
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MsgCode? MessageCode { get; set; }

        /// <summary>
        /// Error of result if any
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Error? Error { get; set; }

        /// <summary>
        /// Implicit cast from result to generic result
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator Result<TModel>(Result<object> result)
        {
            return new Result<TModel>
            {
                StatusCode = result.StatusCode,
                IsSuccess = result.IsSuccess,
                MessageCode = result.MessageCode,
                Error = result.Error
            };
        }

        /// <summary>
        /// Implicit cast from model to generic result
        /// </summary>
        public static implicit operator Result<TModel>(TModel model)
        {
            return new Result<TModel>
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Data = model
            };
        }

        /// <summary>
        /// Create success result with success state
        /// </summary>
        /// <returns>Success result without data</returns>
        public static Result<object> Ok(MsgCode msgCode)
        {
            return new Result<object>
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                MessageCode = msgCode
            };
        }

        /// <summary>
        /// Create success result with success state
        /// </summary>
        /// <returns>Success result without data</returns>
        public static Result<object> Ok()
        {
            return new Result<object>
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true
            };
        }

        /// <summary>
        /// Create success result with success
        /// </summary>
        /// <typeparam name="TEntity">Model data</typeparam>
        /// <param name="value">Data values</param>
        /// <param name="msgCode"></param>
        /// <returns>Success result with data</returns>
        public static Result<TEntity> Ok<TEntity>(TEntity value, MsgCode msgCode) where TEntity : class
        {
            return new Result<TEntity>
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Data = value,
                MessageCode = msgCode
            };
        }

        /// <summary>
        /// Create success result with success
        /// </summary>
        /// <typeparam name="TEntity">Model data</typeparam>
        /// <param name="value">Data values</param>
        /// <returns>Success result with data</returns>
        public static Result<TEntity> Ok<TEntity>(TEntity value) where TEntity : class
        {
            return new Result<TEntity>
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Data = value
            };
        }
    }
}