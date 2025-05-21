using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("/create-order")]
        public static async Task<IResult> CreateOrder([FromBody] CreateOrderRequest request, IMediator mediator, IMapper mapper)
        {

            var command = mapper.Map<CreateOrderRequest>(request);
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPut("/change-status-order")]
        public static async Task<IResult> UpdateOrder(int? id, [FromBody] ChangeStatusOrderRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<ChangeStatusOrderRequest>(request);
            command.Id = id;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpGet("/get-orders")]
        public static async Task<IResult> GetAllOrder(IMediator mediator)
        {
            var command = new GetAllOrderRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-order-by-id")]
        public static async Task<IResult> GetOrderById(int id, IMediator mediator)
        {
            var command = new GetByIdOrderRequest();
            command.Id = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-order-by-customer-id")]
        public static async Task<IResult> GetOrderByCustomerId(int id, IMediator mediator)
        {
            var command = new GetOrderByCustomerIdRequest();
            command.Id = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

    }
}