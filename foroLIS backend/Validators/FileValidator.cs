using FluentValidation;
using foroLIS_backend.DTOs.FileDto;

namespace foroLIS_backend.Validators
{
    public class FileValidator: AbstractValidator<IFormFile>
    {
        public FileValidator() 
        {
            const long Max = 6 * 1024 * 1024; // 6 MB
            string[] extensions = ["png", "jpg", "mp4", "gif", "pdf", "webp", "webm", "av1","jpeg"];

            RuleFor(f => f.Length)
                .Must(f => f != null && f > 0)
                .WithMessage("The file cannot be empty.")
                .Must(f => f <= Max)
                .WithMessage($"The file cannot exceed {Max / (1024 * 1024)} MB.");
            RuleFor(f => f.FileName)
                .Must(f => !extensions.Contains(Path.GetExtension(f).ToLower()))
                .WithMessage($"Only files with extension: {string.Join(", ", extensions)}");
        }
    }
}
