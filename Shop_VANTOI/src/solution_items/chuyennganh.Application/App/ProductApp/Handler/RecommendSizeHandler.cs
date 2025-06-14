using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Handler
{
    public class RecommendSizeHandler : IRequestHandler<RecommendSizeRequest, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(RecommendSizeRequest request, CancellationToken cancellationToken)
        {
            var result = GetSizeWithNote(request.Height, request.Weight, request.Gender);

            return ServiceResponse.Success(
                message: "Đề xuất size thành công",
                data: null,
                query: result
            );
        }

        private SizeResponseDto GetSizeWithNote(int height, int weight, string gender)
        {
            string size = "Không xác định";
            string note = "Không tìm thấy size phù hợp. Vui lòng kiểm tra lại.";

            gender = gender?.ToLower().Trim();

            if (gender == "nam")
            {
                if (height < 160 && weight < 50)
                    (size, note) = ("XS", "Nam vóc dáng nhỏ gọn.");
                else if (height < 165 && weight <= 58)
                    (size, note) = ("S", "Nam thấp – nhẹ.");
                else if (height < 170 && weight <= 66)
                    (size, note) = ("M", "Nam trung bình.");
                else if (height < 175 && weight <= 74)
                    (size, note) = ("L", "Nam cao trung bình.");
                else if (height < 180 && weight <= 82)
                    (size, note) = ("XL", "Nam cao lớn.");
                else if (height < 185 && weight <= 90)
                    (size, note) = ("XXL", "Nam rất cao to.");
                else if (height >= 185 && weight > 90)
                    (size, note) = ("3XL", "Nam ngoại cỡ.");
            }
            else if (gender == "nữ")
            {
                if (height < 150 && weight < 42)
                    (size, note) = ("XS", "Nữ vóc dáng rất nhỏ.");
                else if (height < 155 && weight <= 48)
                    (size, note) = ("S", "Nữ nhỏ gọn.");
                else if (height < 160 && weight <= 55)
                    (size, note) = ("M", "Nữ trung bình.");
                else if (height < 165 && weight <= 62)
                    (size, note) = ("L", "Nữ cao trung bình.");
                else if (height < 170 && weight <= 70)
                    (size, note) = ("XL", "Nữ cao lớn.");
                else if (height >= 170 && weight > 70)
                    (size, note) = ("XXL", "Nữ ngoại cỡ.");
            }

            return new SizeResponseDto
            {
                RecommendedSize = size,
                Note = note
            };
        }
    }
}
