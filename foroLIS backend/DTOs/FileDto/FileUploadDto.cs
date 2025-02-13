namespace foroLIS_backend.DTOs.FileDto
{
    public class FileUploadDto
    {
        public string Message = "File upload success";
        public string Name { get; set; }
        public LinksFile Link { get; set; }
    }

    public class LinksFile
    {
        public string Original {  get; set; } 
        public string? Short { get ; set; }
    }
}
