using FluentValidation;
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
        public PostController(
            ICommonService
            <PostDto, Guid, PostInsertDto, PostUpdateDto> postService
            ,IValidator<PostInsertDto> postInserValidator,
            IValidator<PostUpdateDto> postUpdateValidator)
        {
            _postService = postService;
            _postInsertValidator = postInserValidator;
            _postUpdateValidator = postUpdateValidator;
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
    }
}
