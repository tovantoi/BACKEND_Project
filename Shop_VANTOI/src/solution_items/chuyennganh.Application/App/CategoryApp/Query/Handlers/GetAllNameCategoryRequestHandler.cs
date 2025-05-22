using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Query.Queries;
using chuyennganh.Application.Repositories.CategoryRepo;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Query.Handlers
{
    public class GetAllCategoryNamesRequestHandler : IRequestHandler<GetAllNameCategoryRequest, List<CategoryNameDto>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetAllCategoryNamesRequestHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<List<CategoryNameDto>> Handle(GetAllNameCategoryRequest request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAll();
            var categoryNames = categories.Select(c => new CategoryNameDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return categoryNames;
        }
    }
}
