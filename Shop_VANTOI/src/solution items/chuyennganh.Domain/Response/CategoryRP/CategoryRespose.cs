using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.Response.CategoryRP
{
    public class CategoryRespose : ServiceResponse
    {
        public Category Category { get; }

        public CategoryRespose(Category Category)
        {
            Category = Category;
            IsSuccess = true;
        }
    }
}
