namespace chuyennganh.Application.App.ProductApp.Command
{
    public class ProductInventoryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalStock { get; set; }
        public List<SizeStockDto>? Sizes { get; set; }
    }

    public class SizeStockDto
    {
        public string? SizeLabel { get; set; }
        public int StockQuantity { get; set; }
    }
}
