using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string CoverImage { get; set; }
        public string? VideoUrl { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
