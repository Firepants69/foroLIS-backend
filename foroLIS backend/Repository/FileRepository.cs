using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;

namespace foroLIS_backend.Repository
{
    public class FileRepository: IFileRepository<MediaFile>
    {
        private readonly ApplicationDbContext _context;
        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create( MediaFile file)
        {   
            await _context.MediaFiles.AddAsync(file);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
