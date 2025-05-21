using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Query.Queries;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Query.Handlers
{
    public class GetByIdCategoryRequestHandler : IRequestHandler<GetByIdCategoryRequest, Category>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetByIdCategoryRequestHandler(ICategoryRepository categoryRepository, IMapper mapper, IFileService fileService)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }
        public async Task<Category> Handle(GetByIdCategoryRequest request, CancellationToken cancellationToken)
        {
            Category? category = await categoryRepository.GetByIdAsync(request.Id);
            if (category != null)
            {
                category.ImagePath = string.IsNullOrEmpty(category.ImagePath) ? null : fileService.GetFullPathFileServer(category.ImagePath); 
            }
            if (category is null) category.ThrowNotFound();
            return mapper.Map<Category>(category);
        }
    }
}
