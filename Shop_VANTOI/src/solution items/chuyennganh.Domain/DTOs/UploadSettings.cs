namespace chuyennganh.Domain.DTOs
{
    public class UploadSettings
    {
        public BasePaths BasePaths { get; set; }
    }

    public class BasePaths
    {
        public string Images { get; set; }
        public string Videos { get; set; }
    }
}