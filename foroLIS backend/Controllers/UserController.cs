using FluentValidation;
using foroLIS_backend.DTOs;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace foroLIS_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GoogleService _googleService;
        private readonly IUserService _userService;
        private readonly IValidator<UserRegisterRequestDto> _userValidator;

        public UserController(
            GoogleService googleService
            , IUserService userService
            , IValidator<UserRegisterRequestDto> userValidator)
        {
            _googleService = googleService;
            _userService = userService;
            _userValidator = userValidator;
        }

        [HttpGet("google-user")]
        public async Task<ActionResult<GoogleUserDto>> GetGoogleUser(string token)
        {
            try
            {
                var user = await _googleService.GetUserByToken(token);
                return Ok(user);
            }catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(UserRegisterRequestDto userRegisterRequestDto)
        {
            var validator = _userValidator.Validate(userRegisterRequestDto);
            
            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors);
            }

            try
            {
                var user = await _userService.GoogleLoginAsync(userRegisterRequestDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("loginwithoutgoogle")]
        public async Task<ActionResult<UserResponseDto>> LoginWithoutGoogle(UserLoginRequestDto loginInfo)
        {
            try
            {
                var user = await _userService.LoginAsync(loginInfo);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var response = await _userService.GetByIdAsync(id);
                return Ok(response);
            }catch(Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
