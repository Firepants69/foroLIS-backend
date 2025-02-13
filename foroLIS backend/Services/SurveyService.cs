using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace foroLIS_backend.Services
{
    public class SurveyService : ISurveyService<SurveyDto, SurveyInsertDto,UserFieldSurveyDto,UserFieldInsertSurveyDto>
    {
        ISurveyRepository<Survey, FieldSurvey, UserFieldSurvey, FieldSurveyDto> _surveyRepository;
        IUserService _userService;
        public List<string> Errors { get; }

        public SurveyService(
            IUserService userService,
            ISurveyRepository<Survey, FieldSurvey, UserFieldSurvey, FieldSurveyDto> surveyRepository)
        {
            Errors = new List<string>();
            _surveyRepository = surveyRepository;
            _userService = userService;
        }

       

        public async Task<SurveyDto> Add(SurveyInsertDto surveyInsertDto)
        {
            CurrentUserResponseDto user = null;

            try
            {
                user = await _userService.GetCurrentUserAsync();
            }
            catch (Exception ex)
            {
                Errors.Add("User not found");
                return null;
            }

            var survey = Survey.SurveyInsertToSurvey(surveyInsertDto);
            await _surveyRepository.Add(survey);

            IEnumerable<FieldSurvey> fields = surveyInsertDto.Fields
                .Select(f => FieldSurvey.DtoInsertToFieldSurvey(survey.Id, f));

            await _surveyRepository.AddFields(fields);

            SurveyDto surveyDto = Survey.SurveyToSurveyDto(survey);
            await _surveyRepository.Save();
            var fieldsDto = await _surveyRepository.GetFields(survey.Id, user.Id);

            surveyDto.Fields = fieldsDto;

            return surveyDto;

        }

        public async Task<SurveyDto> GetById(Guid id)
        {
            CurrentUserResponseDto user = null;

            try
            {
                user = await _userService.GetCurrentUserAsync();
            }
            catch (Exception ex)
            {
                Errors.Add("User not found");
                return null;
            }

            Survey survey = await _surveyRepository.GetById(id);
            if (survey == null)
            {
                return null;
            }

            var fields = await _surveyRepository.GetFields(id, user.Id);
            var surveyDto = Survey.SurveyToSurveyDto(survey);
            surveyDto.Fields = fields;
            return surveyDto;
        }

        public async Task<UserFieldSurveyDto> AddUserFieldsSurvey(UserFieldInsertSurveyDto userSurvey)
        {
            CurrentUserResponseDto user = null;

            try
            {
                user = await _userService.GetCurrentUserAsync();
            }catch (Exception ex)
            {
                Errors.Add("User not found");
                return null;
            }
            
            var userField = UserFieldSurvey.DtoInserToModel(userSurvey);
            userField.UserId = user.Id;

            await _surveyRepository.AddUserFieldsSurvey(userField);

            await _surveyRepository.Save();

            return UserFieldSurvey.ModelToDto(userField);
        }

        public  async Task<bool> ValidateUserFields(UserFieldInsertSurveyDto userSurvey)
        {
            CurrentUserResponseDto user = null;

            try
            {
                user = await _userService.GetCurrentUserAsync();
            }
            catch 
            {
                Errors.Add("User not found");
                return false;
            }
            

            // validate that there is no equal one
            // validate that there is no answer in the same survey from the same user
            IEnumerable<UserFieldSurvey> userField = _surveyRepository
                .SearchUserFields(uf => uf.UserId == user.Id && uf.FieldSurveyId == userSurvey.FieldSurveyId);
            
            if(userField.Any())
            {
                Errors.Add("This answer already exists");
                return false;
            }
            var field =await _surveyRepository.getFieldById(userSurvey.FieldSurveyId);
            
            if (!field.Survey.AllowMoreOneAnswer)
            {
                IEnumerable<UserFieldSurvey> userfields = _surveyRepository
                    .SearchUserFields(uf => uf.UserId == user.Id && field.SurveyId == uf.FieldSurvey.SurveyId);
                if (userfields.Any())
                {
                    Errors.Add("Only accepts one response per survey");
                    return false;
                }

            }

            return true;
        }

      

        public async Task<UserFieldSurveyDto> DeleteUserFieldsSurvey(UserFieldInsertSurveyDto userFieldSurvey)
        {
            CurrentUserResponseDto user = null;

            try
            {
                user = await _userService.GetCurrentUserAsync();
            }
            catch (Exception ex)
            {
                Errors.Add("User not found");
                return null;
            }

            var usefieldSearch = _surveyRepository
                .SearchUserFields(uf => uf.FieldSurveyId == userFieldSurvey.FieldSurveyId && uf.UserId == user.Id);
            if(!usefieldSearch.Any())
            {
                Errors.Add("UserField no exists");
                return null;
            }

            var userField = UserFieldSurvey.DtoInserToModel(userFieldSurvey);
            userField.UserId = user.Id;

            _surveyRepository.DeleteUserFieldsSurvey(usefieldSearch.FirstOrDefault());

            await _surveyRepository.Save();

            return UserFieldSurvey.ModelToDto(userField);
        }
    }
}
