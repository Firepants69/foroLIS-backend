using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public class PostRepository : IRepository<Post,PostDto>
    {
        private ApplicationDbContext _context;
        IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PostRepository(ApplicationDbContext context
            , IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Add(Post post)=>
            await _context.Posts.AddAsync(post);

        public void Delete(Post entity)
        =>  _context.Posts.Remove(entity);

        public async Task<IEnumerable<PostDto>> Get(int page =1, int pageSize = 20)
        {
            CurrentUserResponseDto user = null;
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                user = await _userService.GetCurrentUserAsync();
            }

           if(page <= 0) page = 1;
           if(pageSize <= 0) pageSize = 20;

            var posts = await (
                 from post in _context.Posts
                 join survey in _context.Surveys on
                 post.Id equals survey.PostId into postSurveyGroup
                 from sur in postSurveyGroup.DefaultIfEmpty()
                 join field in _context.Fields on 
                 sur.Id equals field.Id into fieldSurveyField
                 from fi in fieldSurveyField.DefaultIfEmpty()

                 select new PostDto 
                 { 
                    ArchitectureOS = post.ArchitectureOS,
                    Title = post.Title,
                    UpdateAt = post.UpdateAt,
                    VersionOS = post.VersionOS,
                    UserId = post.UserId,
                    Content = post.Content,
                    CreateAt = DateTime.Now,
                    FamilyOS = post.FamilyOS,
                    user = new UserPostDto()
                    {
                        ArchitectureOS = post.User.ArchitectureOS,
                        FamilyOS = post.User.FamilyOS,
                        VersionOS = post.User.VersionOS,
                        FirstName = post.User.FirstName,
                        LastName = post.User.LastName,
                        Picture = post.User.Picture,
                    },
                    Id = post.Id,
                    Survey = sur == null ? null : new DTOs.SurveyDtos.SurveyDto
                    {
                        ClosingDate = sur.ClosingDate,
                        Id = sur.Id,
                        Fields = (from field in _context.Fields
                                  join userField in _context.UsersFields
                                  on field.Id equals userField.FieldSurveyId into userFieldGroup
                                  from uf in userFieldGroup.DefaultIfEmpty()
                                  where field.SurveyId == sur.Id
                                  group uf by new { field.Id, field.Title, field.SurveyId } into grouped
                                  select new FieldSurveyDto()
                                  {
                                      Id = grouped.Key.Id,
                                      Title = grouped.Key.Title,
                                      IsSelected = user ==null? false : grouped.Any(uf => uf.UserId == user.Id),
                                      NumberOfSelections = grouped.Count(uf => uf != null),
                                      SuveryId = grouped.Key.SurveyId
                                  })
                    }
                 }
                 )
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

    
            return posts;
        }

        public async Task<IEnumerable<Post>> GetEntity(int page = 1, int pageSize = 20)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;
            
            IEnumerable<Post> posts = await _context.Posts
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
            return posts;
        }

        public async Task<Post> GetById(Guid id)
        => await _context.Posts.FindAsync(id);

        public async Task<PostDto> GetAllPostById(Guid id)
        {
            CurrentUserResponseDto user = null;
            try
            {
                user = await _userService.GetCurrentUserAsync();
            }
            catch
            {
                user = null;
            }

            var searchPost = await (
                 from post in _context.Posts
                 join survey in _context.Surveys on
                 post.Id equals survey.PostId into postSurveyGroup
                 from sur in postSurveyGroup.DefaultIfEmpty()
                 join field in _context.Fields on
                 sur.Id equals field.Id into fieldSurveyField
                 where post.Id == id
                 from fi in fieldSurveyField.DefaultIfEmpty()
                 select new PostDto
                 {
                     ArchitectureOS = post.ArchitectureOS,
                     Title = post.Title,
                     UpdateAt = post.UpdateAt,
                     VersionOS = post.VersionOS,
                     UserId = post.UserId,
                     Content = post.Content,
                     CreateAt = DateTime.Now,
                     FamilyOS = post.FamilyOS,
                     user = new UserPostDto()
                     {
                         ArchitectureOS = post.User.ArchitectureOS,
                         FamilyOS = post.User.FamilyOS,
                         VersionOS = post.User.VersionOS,
                         FirstName = post.User.FirstName,
                         LastName = post.User.LastName,
                         Picture = post.User.Picture,
                     },
                     Id = post.Id,
                     Survey = sur == null ? null : new DTOs.SurveyDtos.SurveyDto
                     {
                         ClosingDate = sur.ClosingDate,
                         Id = sur.Id,
                         Fields = (from field in _context.Fields
                                   join userField in _context.UsersFields
                                   on field.Id equals userField.FieldSurveyId into userFieldGroup
                                   from uf in userFieldGroup.DefaultIfEmpty()
                                   where field.SurveyId == sur.Id
                                   group uf by new { field.Id, field.Title, field.SurveyId } into grouped
                                   select new FieldSurveyDto()
                                   {
                                       Id = grouped.Key.Id,
                                       Title = grouped.Key.Title,
                                       IsSelected = user == null? false : grouped.Any(uf => uf.UserId == user.Id),
                                       NumberOfSelections = grouped.Count(uf => uf != null),
                                       SuveryId = grouped.Key.SurveyId
                                   })
                     }
                 }
                 )
                 .FirstAsync();
            return searchPost;
        }
         

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Post post)
        {
            _context.Posts.Attach(post);
            _context.Posts.Entry(post).State = EntityState.Modified;
        }
    }
}
