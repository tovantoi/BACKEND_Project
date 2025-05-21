using chuyennganh.Domain.Constants;
using chuyennganh.Domain.DependencyInjection.Extensions;
using chuyennganh.Domain.Enumerations;
using System.Net;
using System.Runtime.CompilerServices;

namespace chuyennganh.Domain.ExceptionEx
{
    public class ShopException : Exception
    {
        public bool IsSuccess { get;  set; }
        public int StatusCode { get; set; }
        public List<string>? Errors { get;  set; }
        public MsgCode? MessageCode { get; set; }

        /// <summary>
        ///     Details of which causing exception
        /// </summary>
        public List<string> Details { get; set; } = new();
        public ShopException(int statusCode, List<string> errors)
            : base(errors != null && errors.Any() ? string.Join("; ", errors) : "An error occurred")
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Errors = errors;
            Details = errors;
        }
        public ShopException(int statusCode, MsgCode msgCode, params string[] messages)
        : base(messages != null && messages.Any() ? string.Join("; ", messages) : "An error occurred")
        {
            IsSuccess = false;
            StatusCode = statusCode;
            MessageCode = msgCode;
            Errors = messages?.ToList();
            Details = messages?.ToList();
        }
        // Optional static helper if you still prefer to call like a method
        public static void ThrowException(int statusCode, MsgCode msgCode, params string[] messages)
        {
            throw new ShopException(statusCode, msgCode, messages);
        }

        public static void ThrowNotFoundException(Type? entityType = null, MsgCode msgCode = MsgCode.ERR_NF_FIND_KEY, string? message = null)
        {
            message ??= MsgConst.NOT_FOUND_FIND_KEY.FormatMsg(entityType?.Name ?? MsgConst.ENTITY);
            ThrowException((int)HttpStatusCode.NotFound, msgCode, message);
        }

        /// <summary>
        /// Throw conflict exception
        /// </summary>
        /// <exception cref="CustomException"></exception>
        public static void ThrowConflictException(MsgCode msgCode = MsgCode.ERR_CONFLICT, params string[] messages)
        {
            ThrowException((int)HttpStatusCode.Conflict, msgCode, messages);
        }

        /// <summary>
        /// Throw validation exception
        /// </summary>
        /// <param name="msgCode"></param>
        /// <param name="details"></param>
        public static void ThrowValidationException(MsgCode msgCode = MsgCode.ERR_INVALID, params string[] details)
        {
            ThrowException((int)HttpStatusCode.BadRequest, msgCode, details);
        }
    }
}
