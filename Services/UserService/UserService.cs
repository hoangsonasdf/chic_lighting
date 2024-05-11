using chic_lighting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace chic_lighting.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly chic_lightingContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(chic_lightingContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<string> CreateToken(User user)
        {
            var userRole = await _context.Roles
                .Where(r => r.Id == user.RoleId)
                .Select(r => r.Name)
                .SingleOrDefaultAsync();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userRole)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<User> getCurrentUser()
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }

            string username = principal.FindFirst(ClaimTypes.Name)?.Value;

            return await _context.Users
                .Where(u => u.Username == username)
                .SingleOrDefaultAsync()
                ?? throw new Exception("An error occurred");
        }
    }
}
