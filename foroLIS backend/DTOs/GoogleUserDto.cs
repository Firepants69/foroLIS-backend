using foroLIS_backend.Models;

namespace foroLIS_backend.DTOs
{
    public class GoogleUserDto
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string? picture { get; set; }

        public static Users GoogleUserDtoToUsers(
            GoogleUserDto googleUserDto,
            UserRegisterRequestDto request
            )
        {
            return new Users()
            {
                GoogleId = googleUserDto.id,
                Email = googleUserDto.email,
                EmailConfirmed = googleUserDto.verified_email,
                FirstName = googleUserDto.given_name,
                LastName = googleUserDto.family_name,
                Picture = googleUserDto.picture,
                Language = request.Language,
                Theme = request.Theme,
                FamilyOS = request.FamilyOS,
                VersionOS = request.VersionOS,
                ArchitectureOS = request.ArchitectureOS,
            };
        }
    }

}
