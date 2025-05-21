namespace _365EJSC.ERP.Contract.DependencyInjection.Extensions
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Convert <see cref="Result"/> to proto result <see cref="ProtoResult.CommonResult"/>>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        //public static CommonResult ConvertToCommonResult<T>(this Result<T> result) where T : class
        //{
        //    CommonResult commonResult = new()
        //    {
        //        StatusCode = result.StatusCode,
        //        IsSuccess = result.IsSuccess,
        //        Error = result.Error is not null
        //            ? new Error
        //            {
        //                Details = { result.Error.Details },
        //                StackTrace = result.Error.StackTrace
        //            }
        //            : null
        //    };
        //    if (result.MessageCode is not null) commonResult.MessageCode = result.MessageCode.ToString();
        //    if (result.Data is null)
        //        return commonResult;
        //    commonResult.Data = JsonConvert.SerializeObject(result.Data);
        //    commonResult.DataType = DataType.JSON.ToString();
        //    return commonResult;
        //}
    }
}