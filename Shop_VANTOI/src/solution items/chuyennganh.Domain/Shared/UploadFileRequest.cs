using chuyennganh.Domain.Enumerations;

namespace chuyennganh.Domain.Shared
{
    public class UploadFileRequest
    {
        public string Content { get; set; }
        public string FileName { get; set; }
        public AssetType AssetType { get; set; }
        public Guid ReferenceId { get; set; }
        public string? Suffix { get; set; }
    }
}