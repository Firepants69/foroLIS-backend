using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using foroLIS_backend.DTOs;
using foroLIS_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace foroLIS_backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _secretKey;
        private readonly string? _validIssuer;
        private readonly string? _validAudience;
        private readonly double _expires;
        private readonly UserManager<Users> _userManager;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, UserManager<Users> userManager,
            ILogger<TokenService> logger)
        {
            _userManager = userManager;
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var tokenSecret = Environment.GetEnvironmentVariable("TOKEN_SECRET");
            if (jwtSettings == null || string.IsNullOrEmpty(tokenSecret)) 
            {
                throw new InvalidOperationException("JWT secret ket is not configurated");
            }
            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            _validIssuer = jwtSettings.ValidISsuer;
            _validAudience = jwtSettings.ValidAudience;
            _expires = (double)jwtSettings.Expires;
        }
        public async Task<string> GenerateToken(Users user)
        {
            var singinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(singinCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<List<Claim>> GetClaimsAsync(Users user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("FirstNamme",user.FirstName),
                new Claim("LastName",user.LastName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role, role)));
            return claims;
            
        }

        private JwtSecurityToken GenerateTokenOptions(
            SigningCredentials signingCredentials,
            List<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer:_validIssuer,
                audience:_validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expires),
                signingCredentials: signingCredentials
            ); 
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var refreshToken = Convert.ToBase64String(randomNumber);
            return refreshToken;
        }

    }
}
