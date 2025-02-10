using FluentValidation;
using foroLIS_backend.DTOs.PostDtos;

namespace foroLIS_backend.Validators
{
    public class PostUpdateValidator: AbstractValidator<PostUpdateDto>
    {
        public PostUpdateValidator()
        {
 
            RuleFor(x => x.Title).Length(10, 100).WithMessage("El titulo debe de tener entre 10 y 100 caracteres");RuleFor(x => x.Content).MinimumLength(10).WithMessage("El contenido debe de tener minimo 10 caracteres");
            RuleFor(x => x.Title).Length(10, 100).WithMessage("El titulo debe de tener entre 10 y 100 caracteres");
            RuleFor(x => x.Content)
                .MinimumLength(10).WithMessage("El contenido debe tener al menos 10 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Content));
        }
    }
}
