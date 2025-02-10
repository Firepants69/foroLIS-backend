using FluentValidation;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using foroLIS_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace foroLIS_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService<SurveyDto, SurveyInsertDto, UserFieldSurveyDto, UserFieldInsertSurveyDto> _surveyService;
        private readonly IValidator<SurveyInsertDto> _surveyValidator;
        public SurveyController(ISurveyService<SurveyDto, SurveyInsertDto, UserFieldSurveyDto, UserFieldInsertSurveyDto> surveyService
            , IValidator<SurveyInsertDto> surveyValidator)
        {
            _surveyService = surveyService;
            _surveyValidator = surveyValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<SurveyDto>> GetById(Guid id)
        {
            var survey = await _surveyService.GetById(id);
            if (survey == null)
            {
                return NotFound(_surveyService.Errors);
            }
            
            return survey;
        }
            

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Add(SurveyInsertDto surveyInsertDto)
        {
            var surveyValidation =  await _surveyValidator.ValidateAsync(surveyInsertDto);
            if (!surveyValidation.IsValid)
            {
                return BadRequest(surveyValidation.Errors);
            }
            
            try 
            {
                SurveyDto surveyDto = await _surveyService.Add(surveyInsertDto);
                if (surveyDto == null)
                {
                    return Unauthorized();
                }

                return CreatedAtAction(nameof(GetById), new {id = surveyDto.Id},surveyDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("post-user-survey")]
        [Authorize]
        public async Task<ActionResult<UserFieldSurveyDto>> AddUserField(UserFieldInsertSurveyDto userFieldInsertSurvey)
        {
            try
            {
                var validate = await _surveyService.ValidateUserFields(userFieldInsertSurvey);
                if (!validate)
                {
                    return BadRequest(_surveyService.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            try
            {
                UserFieldSurveyDto dto = await _surveyService.AddUserFieldsSurvey(userFieldInsertSurvey);
                if(dto == null)
                {
                    return Unauthorized();
                }

                return Ok(dto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete-user-survey")]
        [Authorize]
        public async Task<ActionResult<UserFieldSurveyDto>> DeleteUserField(UserFieldInsertSurveyDto userFieldInsertSurvey)
        {
            try
            {
                UserFieldSurveyDto dto = await _surveyService
                    .DeleteUserFieldsSurvey(userFieldInsertSurvey);
                if (dto == null)
                {
                    if (_surveyService.Errors.Contains("User not found"))
                    {
                        return Unauthorized(_surveyService.Errors);

                    }
                    return NotFound(_surveyService.Errors);
                }
               return Ok(dto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
