using foroLIS_backend.DTOs;

namespace foroLIS_backend.Services
{
    public interface ICommonService<T,TID,TI,TU>
    {
        Task<IEnumerable<T>> Get(int page, int pageSize);
        Task<T> GetById(TID id);
        Task<T> Add(TI InsertDto);
        Task<T> Update(TU UpdateDto);
        Task<T> Delete(TID id);

    }
}
