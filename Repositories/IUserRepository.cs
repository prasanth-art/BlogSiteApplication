using BlogSiteApplication.Models;

namespace BlogSiteApplication.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string id);
    }
}
