using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace foroLIS_backend.Services
{
    public class FileService
    {
        private readonly string _route = Path.Combine(Directory.GetCurrentDirectory(), "FilesUploaded");
        private readonly string[] _extensions_shorts = [".png", ".jpg", ".webp"]; // Extensiones con punto
        private readonly IFileRepository<MediaFile> _fileRepository;
        public FileService(IFileRepository<MediaFile> fileRepository ) 
        {
            _fileRepository = fileRepository;   
        }

        public async Task<FileUploadDto> UploadFile(IFormFile file)
        {
            if (!Directory.Exists(_route))
            {
                Directory.CreateDirectory(_route);
            }

            var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var route = Path.Combine(_route, name);

            using (var stream = new FileStream(route, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            bool isImage = _extensions_shorts.Contains(Path.GetExtension(file.FileName.ToLower()));
            
            string shortPath = "";

            // test
            if (isImage)
            {
                // shortname
                var shortFileName = "short_" + name;
                shortPath = Path.Combine(_route, shortFileName);

                using (var image = await Image.LoadAsync(route)) {
                    
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(500, 500), 
                        Mode = ResizeMode.Max      
                    }));

                    await image.SaveAsync(shortPath);
                }
            }


            return new FileUploadDto()
            {
                Name = name,
                Link = new LinksFile
                {
                    Original = route,
                    Short = isImage ? shortPath : null
                }
            };
        }
        
    }
}