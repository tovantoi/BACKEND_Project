namespace chuyennganh.Application.App.Thong_Ke.Dto
{
    public class ProductRevenueDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
