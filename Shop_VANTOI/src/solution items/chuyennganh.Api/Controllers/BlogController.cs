using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        [HttpPut("/delete-blog")]
        public static async Task<IResult> DeleteBlog(int id, [FromBody] DeleteBlogRequest request, IMediator mediator)
        {
            request.Id = id;
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPost("/create-blog")]
        public static async Task<IResult> CreateBlog([FromBody] CreateBlogRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<CreateBlogRequest>(request);
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }

        [HttpPut("/update-blog")]
        public static async Task<IResult> UpdateBlog(int? id, [FromBody] UpdateBlogRequest request, IMediator mediator, IMapper mapper)
        {
            request.Id = id;
            var results = await mediator.Send(request);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return Results.BadRequest(new { isSuccess = false, message = "Lỗi chi tiết" });
        }

        [HttpGet("/get-blogs")]
        public static async Task<IResult> GetAllBlog(IMediator mediator, IMapper mapper)
        {
            var command = new GetAllBlogRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-detail-blog")]
        public static async Task<IResult> GetDetailBlog(int id, IMediator mediator, IMapper mapper)
        {
            var command = new GetDetailBlogRequest();
            command.Id = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }
    }
 }
