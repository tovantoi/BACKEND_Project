using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chuyennganh.Api.Controllers
{
    [ApiController]
    [Route("minimal/api")]
    public class RealTimeDataController : ControllerBase
    {
        [HttpGet("get-real-time-data")]
        public async Task<IActionResult> GetRealTimeData()
        {
            // Giả lập dữ liệu để trả về
            var data = new
            {
                lineChartData = new List<int> { 50, 70, 90, 80, 100, 120 }, // Doanh thu theo tháng
                doughnutChartData = new List<int> { 75, 25 }, // Chi phí marketing và bán hàng
                barChartData = new List<int> { 10, 15, 12, 8, 7, 18 } // Doanh thu theo kênh
            };

            return Ok(data);
        }
    }
}
