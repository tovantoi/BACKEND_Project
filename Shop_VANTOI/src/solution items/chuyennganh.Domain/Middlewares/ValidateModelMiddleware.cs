using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Json;

namespace chuyennganh.Domain.Middlewares
{
    public class ValidateModelMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateModelMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}, Content-Type: {context.Request.ContentType}");

            // Tiếp tục pipeline
            await _next(context);

            //Console.WriteLine($"After pipeline: StatusCode={context.Response.StatusCode}, HasStarted={context.Response.HasStarted}");

            // Xử lý các mã lỗi
            Result<object> result = null;

            switch (context.Response.StatusCode)
            {
                case (int)HttpStatusCode.UnsupportedMediaType: // 415
                    result = new Result<object>
                    {
                        MessageCode = MsgCode.ERR_UNSUPPORTED_MEDIA_TYPE,
                        StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                        Error = new Errors.Error("UNSUPPORTED_MEDIA_TYPE",
                            new[] { $"The media type '{context.Request.ContentType}' is not supported." })
                    };
                    break;

                case (int)HttpStatusCode.NotFound: // 404
                    result = new Result<object>
                    {
                        MessageCode = MsgCode.ERR_NOT_FOUND,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Error = new Errors.Error("ROUTE_NOT_FOUND",
                            new[] { "The requested endpoint was not found." })
                    };
                    break;

                default:
                    // Kiểm tra ModelState nếu request đến controller
                    var controllerActionDescriptor = context.Features.Get<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
                    if (controllerActionDescriptor != null)
                    {
                        var modelState = context.Features.Get<ModelStateDictionary>();
                        if (modelState != null && !modelState.IsValid)
                        {
                            var errorMessages = modelState
                                .Where(m => m.Value.Errors.Count > 0)
                                .SelectMany(m => m.Value.Errors.Select(e => $"{m.Key}: {e.ErrorMessage}"))
                                .ToList();

                            result = new Result<object>
                            {
                                MessageCode = MsgCode.ERR_BAD_REQUEST,
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Error = new Errors.Error(null, errorMessages.ToArray())
                            };
                        }
                    }
                    break;
            }

            // Nếu có lỗi, ghi response
            if (result != null)
            {
                await WriteErrorResponse(context, result);
                return;
            }
        }

        private async Task WriteErrorResponse(HttpContext context, Result<object> result)
        {
            //Console.WriteLine($"Before write: HasStarted={context.Response.HasStarted}, StatusCode={context.Response.StatusCode}");
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = result.StatusCode;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                var json = JsonSerializer.Serialize(result);
                //Console.WriteLine($"Writing response: {json}");
                await context.Response.WriteAsync(json);
                //Console.WriteLine("After write body");
            }
        }

        /*
        public async Task InvokeAsync(HttpContext context)
        {
            // Tiếp tục pipeline
            await _next(context);


            // Kiểm tra nếu request không khớp với endpoint nào
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                var result = new Result<object>
                {
                    MessageCode = MsgCode.ERR_NOT_FOUND,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Error = new Errors.Error("ROUTE_NOT_FOUND", new[] { "The requested endpoint was not found." })
                };

                context.Response.StatusCode = result.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                return;
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.UnsupportedMediaType)
            {
                var result = new Result<object>
                {
                    MessageCode = MsgCode.ERR_UNSUPPORTED_MEDIA_TYPE,
                    StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                    Error = new Errors.Error("UNSUPPORTED_MEDIA_TYPE",
                                new[] { $"The media type '{context.Request.ContentType}' is not supported." })
                };

                    context.Response.StatusCode = result.StatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                return;
            }

            // Kiểm tra ModelState nếu request đến controller
            var controllerActionDescriptor = context.Features.Get<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
            if (controllerActionDescriptor != null)
            {
                var modelState = context.Features.Get<ModelStateDictionary>();
                if (modelState != null && !modelState.IsValid)
                {
                    var errorMessages = modelState
                        .Where(m => m.Value.Errors.Count > 0)
                        .SelectMany(m => m.Value.Errors.Select(e => $"{m.Key}: {e.ErrorMessage}"))
                        .ToList();

                    var result = new Result<object>
                    {
                        MessageCode = MsgCode.ERR_INTERNAL_SERVER,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Error = new Errors.Error(null, errorMessages.ToArray())
                    };

                    context.Response.StatusCode = result.StatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
            }
        }
        */
    }

    // Extension method để đăng ký middleware
    public static class ValidateModelMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidateModel(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateModelMiddleware>();
        }
    }
}
