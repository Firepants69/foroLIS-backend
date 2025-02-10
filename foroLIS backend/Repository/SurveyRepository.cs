using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public class SurveyRepository: ISurveyRepository<Survey,FieldSurvey,UserFieldSurvey,FieldSurveyDto>
    {
        private readonly ApplicationDbContext _context;
        public SurveyRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task AddFields (   IEnumerable<FieldSurvey> Fields)
        => await _context.Fields.AddRangeAsync(Fields);
        

        public async Task<Survey> GetById (Guid Id)
         => await _context.Surveys.FindAsync(Id);
        
        public async Task Add(Survey survey) =>
            await _context.Surveys.AddAsync(survey);

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<FieldSurvey> getFieldById(Guid fieldSurveyId)
        {
            return await _context.Fields
                .Include(fs => fs.Survey) 
                .FirstOrDefaultAsync(fs => fs.Id == fieldSurveyId);
        }

        public async Task<IEnumerable<FieldSurveyDto>> GetFields(Guid SurveyId, string userId)

        {
            var result = await (from field in _context.Fields
                join userField in _context.UsersFields
                on field.Id equals userField.FieldSurveyId into userFieldGroup
                from uf in userFieldGroup.DefaultIfEmpty() 
                where field.SurveyId == SurveyId 
                group uf by new { field.Id, field.Title, field.SurveyId } into grouped
                select new FieldSurveyDto()
                {
                    Id = grouped.Key.Id,
                    Title = grouped.Key.Title,
                    IsSelected = grouped.Any(uf => uf.UserId == userId), 
                    NumberOfSelections = grouped.Count(uf => uf != null), 
                    SuveryId = grouped.Key.SurveyId
                })
               .ToListAsync();


            return result;
        }

        public async Task AddUserFieldsSurvey(UserFieldSurvey userSurvey)
        => await _context.UsersFields.AddAsync(userSurvey);

 
        public void DeleteUserFieldsSurvey(UserFieldSurvey userFieldSurvey)
        =>   _context.UsersFields.Remove(userFieldSurvey);

        public IEnumerable<Survey> SearchSurveys(Func<Survey, bool> filter) =>
           _context.Surveys
            .Include(s=> s.Post)
            .Where(filter)
           .ToList();

        public IEnumerable<UserFieldSurvey> SearchUserFields(Func<UserFieldSurvey, bool> filter) =>
           _context.UsersFields
            .Include(uf => uf.FieldSurvey.Survey.Post)
            .Where(filter)
           .ToList();

        public IEnumerable<FieldSurvey> SearchFields(Func<FieldSurvey, bool> filter) =>
           _context.Fields
            .Include(f=> f.Survey.Post)
            .Where(filter)
           .ToList();

    }
}
