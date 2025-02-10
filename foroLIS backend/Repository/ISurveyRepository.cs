using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public interface ISurveyRepository<T,TL,TX,TDTO>
    {
        public Task AddFields(IEnumerable<TL> elements);

        public Task<T> GetById(Guid Id);

        public  Task Add(T element);

        public Task Save();

        public Task<IEnumerable<TDTO>> GetFields(Guid SuveryId, string userId);

        public Task AddUserFieldsSurvey(TX userSurvey);

        public void DeleteUserFieldsSurvey(TX userFieldSurvey);

        public IEnumerable<T> SearchSurveys(Func<T, bool> filter);

        public IEnumerable<TX> SearchUserFields(Func<TX, bool> filter);

        public IEnumerable<TL> SearchFields(Func<TL, bool> filter);
        public Task<TL> getFieldById(Guid id);


    }
}
