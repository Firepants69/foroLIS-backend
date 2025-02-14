using FluentValidation;
using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace foroLIS_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        ICommonService<PostDto, Guid, PostInsertDto, PostUpdateDto> _postService;
        IValidator<PostInsertDto> _postInsertValidator;
        IValidator<PostUpdateDto> _postUpdateValidator;
        FileService _fileService;
        IValidator<IFormFile> _fileValidator;
        public PostController(
            ICommonService
            <PostDto, Guid, PostInsertDto, PostUpdateDto> postService
            ,IValidator<PostInsertDto> postInserValidator,

            IValidator<PostUpdateDto> postUpdateValidator,
            FileService fileService
,
            IValidator<IFormFile> fileValidator)
        {
            _postService = postService;
            _postInsertValidator = postInserValidator;
            _postUpdateValidator = postUpdateValidator;
            _fileService = fileService;
            _fileValidator = fileValidator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetById(Guid id)
        {
            try
            {
                return await _postService.GetById(id);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostDto>> Add(PostInsertDto postInsertDto)
        {
            try
            {
                var validationResult = await _postInsertValidator.ValidateAsync(postInsertDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var result = await _postService.Add(postInsertDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> Get(int page,int pageSize)
        {
            try
            {
                IEnumerable<PostDto> posts = await _postService.Get(page, pageSize);
                return Ok(posts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<PostDto>> Update(PostUpdateDto postUpdateDto)
        {
            try 
            {
                var validationResult = await _postUpdateValidator.ValidateAsync(postUpdateDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var response = await _postService.Update(postUpdateDto);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("upload")]
        [RequestSizeLimit(6 * 1024 * 1024)] 
        public async Task<ActionResult<FileUploadDto>> UploadFile( IFormFile file)
        {
            var validator = await _fileValidator.ValidateAsync(file);
            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors);
            }

            try
            {
               var result = await _fileService.UploadFile(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Conflict(ex.Message);
            }
        }
    }
}
