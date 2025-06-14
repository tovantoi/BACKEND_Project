using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Command
{
    public class CreateProductImageCommand : IRequest<ServiceResponse>
    {
        public int ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public int? StockQuantity { get; set; }
        public int SortOrder { get; set; }
    }
}
