using AutoMapper;
using chuyennganh.Application.App.SendOrderEmail.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SendOrderEmailController : ControllerBase
    {
        [HttpPost("/send-order-email")]
        public static async Task<IResult> SendOrderEmail([FromBody] SendOrderEmailCommand request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<SendOrderEmailCommand>(request);
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

    }
}
