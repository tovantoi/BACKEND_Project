using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Enumerations
{
    /// <summary>
    /// Define type of response data
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DataType
    {
        /// <summary>
        /// Response is json format
        /// </summary>
        JSON,

        /// <summary>
        /// Response is graphQL format
        /// </summary>
        GRAPHQL,

        /// <summary>
        /// Response is XML format
        /// </summary>
        XML,

        /// <summary>
        /// Response is gRPC format
        /// </summary>
        GRPC
    }
}