using FluentValidation;
using foroLIS_backend.DTOs;

namespace foroLIS_backend.Validators
{
    public class UserRegisterRequestValidator: AbstractValidator<UserRegisterRequestDto>
    {
        public UserRegisterRequestValidator() 
        {
            RuleFor(user => user.GoogleToken).NotEmpty().WithMessage("you need a google token");
            RuleFor(user => user.ArchitectureOS).Length(10,30)
                .When(user=> user.ArchitectureOS != null)
                .WithMessage("ArchitectureOS it must have a length between 10 and 30");
            RuleFor(user => user.VersionOS).Length(2, 10)
                .When(user => user.VersionOS!= null)
                .WithMessage("VersionOS it must have a length between 5 and 10");
            RuleFor(user => user.FamilyOS).Length(10, 20)
                .When(user => user.FamilyOS != null)
                .WithMessage("FamilyOS it must have a length between 10 and 20");
            RuleFor(user => user.Theme).Length(5,7)
                .When(user => user.Theme != null)
                .WithMessage("theme it must have a length between 5,7");
            RuleFor(user => user.Language).Length(2,3)
                .When(user => user.Language != null)
                .WithMessage("Language it must have 3 of length ");


        }

    }
}
