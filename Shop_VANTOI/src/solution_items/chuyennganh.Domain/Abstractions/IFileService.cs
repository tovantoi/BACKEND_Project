using chuyennganh.Domain.Shared;

namespace chuyennganh.Domain.Abstractions
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(UploadFileRequest uploadFileSetting);
        string GetFullPathFileServer(string? filePath);
    }
}