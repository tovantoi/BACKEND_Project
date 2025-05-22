using AutoMapper;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersAddressController : ControllerBase
    {

        [HttpGet("/get-customeraddress-by-id")]
        public static async Task<IResult> GetByIdCustomerAddress(int id, IMediator mediator, IMapper mapper)
        {
            var command = new GetByIdCustomerAddressRequest();
            command.Id = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-customeraddresss")]
        public static async Task<IResult> GetAllCustomerAddress(IMediator mediator, IMapper mapper)
        {
            var command = new GetAllCustomerAddressRequest();
            var result = await mediator.Send(command);
            if (result != null && result.Any())
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.NotFound("Không có sản phẩm nào.");
        }

        [HttpPost("/create-customeraddress")]
        public static async Task<IResult> CreateCustomerAddress(
            [FromBody] CustomerAddressCreateRequest request,
            IMediator mediator,
            IMapper mapper)
        {
            var command = mapper.Map<CustomerAddressCreateRequest>(request);
            var results = await mediator.Send(command);

            if (results.IsSuccess)
            {
                return TypedResults.Ok(new
                {
                    results.IsSuccess,
                    results.Message,
                    Query = results.Query 
                });
            }

            return TypedResults.BadRequest(new
            {
                results.IsSuccess,
                results.Message,
                results.Errors
            });
        }


        [HttpPut("/update-customeraddress")]
        public static async Task<IResult> UpdateCustomerAddress(int? id, [FromBody] CustomerAddressUpdateRequest request, IMediator mediator, IMapper mapper)
        {

            var command = mapper.Map<CustomerAddressUpdateRequest>(request);
            command.Id = id;
            var results = await mediator.Send(command);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return TypedResults.BadRequest(results);
        }

        [HttpGet("/get-address-by-customer-id")]
        public static async Task<IResult> GetCustomerAddressByCustomerId(int? id, IMediator mediator, IMapper mapper)
        {

            var command = new GetCustomerAddressByCustomerIdRequest();
            command.CustomerId = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }
    }
}
