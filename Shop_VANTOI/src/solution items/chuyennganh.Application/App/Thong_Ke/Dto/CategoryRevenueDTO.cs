namespace chuyennganh.Application.App.Thong_Ke.Dto
{
    public class CategoryRevenueDTO
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
