namespace chuyennganh.Application.App.Thong_Ke.Dto
{
    public class SalesSummaryDTO
    {
        public int TotalProductsSold { get; set; }
        public decimal TotalRevenueFromNewCustomers { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal Profit => TotalRevenueFromNewCustomers - EstimatedCost;
    }

}
