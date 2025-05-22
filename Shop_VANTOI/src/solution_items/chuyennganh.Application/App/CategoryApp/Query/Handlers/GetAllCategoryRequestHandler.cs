using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Query.Queries;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Services;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Query.Handlers
{
    public class GetAllCategoryRequestHandler : IRequestHandler<GetAllCategoryRequest, List<Category>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetAllCategoryRequestHandler(ICategoryRepository categoryRepository, IMapper mapper, IFileService fileService)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<List<Category>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = categoryRepository.FindAll();
            var employees = await Task.Run(() =>
            {
                return category
                    .Select(c => new
                    {
                        Category = c,
                        ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : fileService.GetFullPathFileServer(c.ImagePath)
                    })
                    .ToList();
            }, cancellationToken);

            var result = employees
                .Select(x =>
                {
                    x.Category.ImagePath = x.ImagePath;
                    return x.Category;
                })
                .ToList();
            return mapper.Map<List<Category>>(category);
        }
    }
}
