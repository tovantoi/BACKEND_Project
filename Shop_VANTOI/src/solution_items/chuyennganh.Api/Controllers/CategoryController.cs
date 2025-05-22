using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CategoryApp.Query.Queries;
using chuyennganh.Application.App.ProductApp.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpDelete("/delete-category")]
        public static async Task<IResult> Delete(int id, IMediator mediator)
        {
            var command = new DeleteCategoryRequest { Id = id };
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPost("/create-category")]
        public static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request, IMediator mediator, IMapper mapper)
        {
                var command = mapper.Map<CreateCategoryRequest>(request);
                var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPut("/update-category")]
        public static async Task<IResult> UpdateCategory(int? id, [FromBody] UpdateCategoryRequest request, IMediator mediator, IMapper mapper)
        {
            request.Id = id;
            var results = await mediator.Send(request);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return Results.BadRequest(new { isSuccess = false, message = "Lỗi chi tiết" });
        }

        [HttpGet("/get-category-by-id")]
        public static async Task<IResult> GetByIdCategory(int id, IMediator mediator, IMapper mapper)
        {
                var command = new GetByIdCategoryRequest();
                command.Id = id;
                var result = await mediator.Send(command);
                return TypedResults.Ok(result);
        }

        [HttpGet("/get-categories")]
        public static async Task<IResult> GetAllCategory(IMediator mediator, IMapper mapper)
        {
                var command = new GetAllCategoryRequest();
                var result = await mediator.Send(command);
                return TypedResults.Ok(result);
        }

        [HttpGet("/get-name-categories")]
        public static async Task<IResult> GetAllNameCategory(IMediator mediator, IMapper mapper)
        {
            var command = new GetAllNameCategoryRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }
    }
}
