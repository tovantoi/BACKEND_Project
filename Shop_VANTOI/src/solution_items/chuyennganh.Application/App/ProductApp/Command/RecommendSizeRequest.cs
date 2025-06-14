using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Command
{
    public class RecommendSizeRequest : IRequest<ServiceResponse>
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Gender { get; set; }
    }
}
