using chic_lighting.Models;

namespace chic_lighting.Services.UserService
{
    public interface IUserService
    {
        Task<string> CreateToken(User user);
        Task<User> getCurrentUser();
    }
}
