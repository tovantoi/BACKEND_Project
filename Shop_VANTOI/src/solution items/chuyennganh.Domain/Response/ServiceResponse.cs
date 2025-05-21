using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace chuyennganh.Application.Response
{
    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Data { get; set; }
        public object? Query { get; set; }

        // Static method for successful response
        public static ServiceResponse Success(string message, string? token = null, object? query = null)
        {
            return new ServiceResponse
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Message = message,
                Query = query,
                Errors = null
            };
        }

        // Static method for failure response
        public static ServiceResponse Failure(string message, List<string>? errors = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message,
                Errors = errors,
            };
        }

        // Overload for failure with a single error message
        public static ServiceResponse Failure(string message, string error)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { error },
            };
        }
    }
}
