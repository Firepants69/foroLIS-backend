using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Services
{
    public interface ISurveyService<T,TA, TDTO,TDTOI>
    {
        public List<string> Errors { get; }
        public Task<T> Add(TA element);
        public Task<T> GetById(Guid id);
        public Task<TDTO> AddUserFieldsSurvey(TDTOI userSurvey);
        public Task<TDTO> DeleteUserFieldsSurvey(TDTOI userFieldSurvey);
        public Task<bool> ValidateUserFields(UserFieldInsertSurveyDto userSurvey);

    }
}
