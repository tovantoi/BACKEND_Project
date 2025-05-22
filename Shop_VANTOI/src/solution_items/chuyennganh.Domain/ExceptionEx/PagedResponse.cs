using System.Text.Json.Serialization;

namespace chuyennganh.Domain.ExceptionEx
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        [JsonIgnore]
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        [JsonIgnore]
        public bool HasPreviousPage => PageNumber > 1;

        [JsonIgnore]
        public bool HasNextPage => PageNumber < TotalPages;

        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalCount)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }

}