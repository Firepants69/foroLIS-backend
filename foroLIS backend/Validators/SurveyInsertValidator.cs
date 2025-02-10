using FluentValidation;
using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class SurveyInsertValidator:AbstractValidator<SurveyInsertDto>
    {
        private readonly ApplicationDbContext _context;
        public SurveyInsertValidator(ApplicationDbContext context) 
        {
            _context = context;
            RuleFor(r => r.Fields).Must(f => f.Count >= 2 && f.Count <= 12)
                .WithMessage("The survey must contain at least 2 fields and a maximum of 12");
            RuleFor(r => r.PostId)
                .NotEmpty().WithMessage("postId is required")
                .MustAsync(PostExists).WithMessage("PostNoExists");
            RuleFor(r => r.PostId)
                .MustAsync(ExistsSurvey).WithMessage("Only one survey per post");

        }
        private async Task<bool> PostExists(Guid postId,CancellationToken cancellationToken)
        {
            return await _context.Posts.AnyAsync(p=> p.Id == postId,cancellationToken);
        }
        private async Task<bool> ExistsSurvey(Guid postId, CancellationToken cancellationToken)
        {
            var result = await _context.Surveys.AnyAsync(s => s.PostId == postId, cancellationToken);
            return !result;
        }
    }
}
