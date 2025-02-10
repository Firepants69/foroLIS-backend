using FluentValidation;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Models;

namespace foroLIS_backend.Validators
{
    public class PostInsertValidator: AbstractValidator<PostInsertDto>
    {
        public PostInsertValidator() 
        {
            RuleFor(post => post.Title)
                .NotEmpty().WithMessage("El titulo no puede estar vacio");
            RuleFor(x => x.Title).Length(10, 100).WithMessage("El titulo debe de tener entre 10 y 100 caracteres"); RuleFor(x => x.Content).MinimumLength(10).WithMessage("El contenido debe de tener minimo 10 caracteres");
            RuleFor(x => x.Title).Length(10, 100).WithMessage("El titulo debe de tener entre 10 y 100 caracteres");
            RuleFor(x => x.Content)
                .MinimumLength(10).WithMessage("El contenido debe tener al menos 10 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Content));

        }
    }
}
