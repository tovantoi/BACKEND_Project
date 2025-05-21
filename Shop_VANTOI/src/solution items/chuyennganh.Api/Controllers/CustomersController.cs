using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Customers.Query.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CustomersController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
        [HttpDelete("/delete-customer")]
        public static async Task<IResult> Delete(int id, IMediator mediator)
        {
            var command = new DeleteCustomerRequest { Id = id };
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPost("/register-customer")]
        public static async Task<IResult> RegisterCustomer([FromBody] RegisterRequest request, IMediator mediator)
        {

            var results = await mediator.Send(request);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return TypedResults.BadRequest(results);
        }

        [HttpPost("/register-google")]
        public static async Task<IResult> RegisterGoogle([FromBody] GoogleRegisterRequest request, IMediator mediator)
        {

            var results = await mediator.Send(request);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return TypedResults.BadRequest(results);
        }

        [HttpPost("/login-customer")]
        public static async Task<IResult> Login([FromBody] LoginRequest request, IMediator mediator)
        {
            var response = await mediator.Send(request);

            if (!response.IsSuccess)
            {
                return TypedResults.BadRequest(response); 
            }

            return TypedResults.Ok(response);
        }


        [HttpPut("/authen-customer")]
        public static async Task<IResult> AuthenCustomer(string? email, [FromBody] AuthenCustomerRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<AuthenCustomerRequest>(request);
            command.Email = email;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPost("/resend-otp")]
        public static async Task<IResult> ResendOTP(string? email, [FromBody] ResendOTPRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<ResendOTPRequest>(request);
            command.Email = email;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPost("/customer-logout")]
        public static async Task<IResult> Logout(LogoutCustomerRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<LogoutCustomerRequest>(request);
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPut("/update-profile-customer")]
        public static async Task<IResult> UpdateCustomerProfile(int? id, [FromBody] UpdateProifleCustomerRequest request, IMediator mediator, IMapper mapper)
        {

            var command = mapper.Map<UpdateProifleCustomerRequest>(request);
            command.Id = id;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpGet("/get-customer-by-id")]
        public static async Task<IResult> GetCustomerById(int id, IMediator mediator)
        {
            var command = new GetCustomerByIdCustomerRequest { Id = id };

            try
            {
                var customer = await mediator.Send(command);
                if (customer == null)
                {
                    return TypedResults.BadRequest(new
                    {
                        IsSuccess = false,
                        Message = "Customer not found",
                        Data = (object?)null
                    });
                }
                return TypedResults.Ok(new
                {
                    IsSuccess = true,
                    Message = "Customer retrieved successfully",
                    Data = customer
                });
            }
            catch (Exception ex)
            {
                return TypedResults.NotFound(new
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = (object?)null
                });
            }
        }


        [HttpGet("/get-customers")]
        public static async Task<IResult> GetAllCustomerRoles(IMediator mediator)
        {
            var command = new GetAllCustomerRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpPut("/change-password")]
        public static async Task<IResult> ChangePassword(string? email, IMediator mediator)
        {
            var command = new ChangePasswordRequest();
            command.Email = email;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }
        [HttpPut("/update-customer-password")]
        public static async Task<IResult> UpdateCustomerPassword(string? email, [FromBody] UpdateCustomerPasswordRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<UpdateCustomerPasswordRequest>(request);
            command.Email = email;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }
    }
}

