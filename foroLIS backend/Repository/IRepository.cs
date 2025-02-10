using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Models;

namespace foroLIS_backend.Repository
{
    public interface IRepository<TEntity,TEntityDto>
    {
        Task<IEnumerable<TEntity>> GetEntity(int page, int pageSize);
        Task<IEnumerable<TEntityDto>> Get(int page, int pageSize);
        Task<TEntity> GetById(Guid id);
        Task<TEntityDto> GetAllPostById(Guid id);
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Save();

    }
}
