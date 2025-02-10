using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace foroLIS_backend.Services
{
    public class PostService :
        ICommonService<PostDto, Guid, PostInsertDto, PostUpdateDto>
    {
        IRepository<Post,PostDto> _repository;
        IUserService _userService;

        public PostService(
            IRepository<Post, PostDto> repository,
            IUserService userService
            )
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task<PostDto> Add(PostInsertDto postInsertDto)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            
            Post newPost = Post.InsertDtoToPost(postInsertDto,currentUser.Id);

            await _repository.Add(newPost);
            await _repository.Save();
            var postDto = Post.postToPostDto(newPost);
            return postDto;

        }

        public async Task<PostDto> Delete(Guid id)
        {
            Post post = await _repository.GetById(id);
            
            if (post == null)
            {
                return null;
            }

            _repository.Delete(post);
            await _repository.Save();
            return Post.postToPostDto(post);
        }

        public async Task<IEnumerable<PostDto>> Get(int page, int pageSize)
        {
           IEnumerable<PostDto> posts = await _repository.Get(page, pageSize);
            return posts;
        }

        public async Task<PostDto> GetById(Guid id)
        {
            var post = await _repository.GetAllPostById(id);
            return post;
        }

        public async Task<PostDto> Update(PostUpdateDto postUpdateDto)
        {
            var post = await _repository.GetById(postUpdateDto.Id);
            if(post == null)
            {
                return null;
            }
            post.Content = postUpdateDto.Content ?? post.Content;
            post.Title = postUpdateDto.Title ?? post.Title;
            post.UpdateAt = DateTime.Now;
            _repository.Update(post);
            await _repository.Save();
            var postDto = Post.postToPostDto(post);
            return postDto;
        }
    }
}
