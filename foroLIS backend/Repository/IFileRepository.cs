using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public interface IFileRepository<TEntity>
    {
        public Task Create(TEntity file);

        public Task Save();
        
    }
}
