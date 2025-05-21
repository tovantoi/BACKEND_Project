using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Errors;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Helpers;
using chuyennganh.Domain.Shared;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace chuyennganh.Domain.DependencyInjection.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Convert <see cref="Exception"/> to <see cref="Result"/>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Result<object> ConvertToResult(this Exception exception)
        {
            // Check current environment
            bool isProduction = EnvironmentHelper.Environment.IsProduction();
            // Cast exception to custom exception
            ShopException? customException = exception as ShopException;
            // Convert exception to result
            return new Result<object>
            {
                MessageCode = customException?.MessageCode ?? MsgCode.ERR_INTERNAL_SERVER,
                StatusCode = customException?.StatusCode ?? (int)HttpStatusCode.InternalServerError,
                Error = isProduction ? null : new Error(exception.StackTrace ?? string.Empty, customException?.Details?.ToArray() ?? [exception.Message])
            };
        }

        ///// <summary>
        ///// Convert <see cref="Exception"/> to <see cref="ProtoResult.CommonResult"/>
        ///// </summary>
        ///// <param name="exception"></param>
        ///// <returns></returns>
        //public static CommonResult ConvertToCommonResult(this Exception exception)
        //{
        //    return exception.ConvertToResult().ConvertToCommonResult();
        //}
    }
}