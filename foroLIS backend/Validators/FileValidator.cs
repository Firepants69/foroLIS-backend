using FluentValidation;
using foroLIS_backend.DTOs.FileDto;

namespace foroLIS_backend.Validators
{
    public class FileValidator: AbstractValidator<FileInsertDto>
    {
        public FileValidator() 
        {
            const long Max = 6 * 1024 * 1024; // 6 MB
            string[] extensions = ["png","jpg","mp4","gif","pdf","webp", "webm", "av1"];

            RuleFor(f => f.formFile)
                .Must(f => f != null && f.Length > 0)
                .WithMessage("The file cannot be empty.")
                .Must(f => f.Length <= Max)
                .WithMessage($"The file cannot exceed {Max / (1024 * 1024)} MB.")
                .Must(f => extensions.Contains(Path.GetExtension(f.FileName).ToLower()))
                .WithMessage($"Only files with extension: {string.Join(", ", extensions)}");
        }
    }
}
