using BlogSiteApplication.DTO;
using BlogSiteApplication.Models;

namespace BlogSiteApplication.Repositories
{
    public interface IBlogRepository
    {
        Task AddBlogAsync(Blog blog);
        Task<IEnumerable<Blog>> GetBlogsByCategoryAsync(string category);
        Task<IEnumerable<Blog>> GetBlogsByUserAsync(string userName);
        Task<Blog> GetBlogByIdAsync(string id);
        Task DeleteBlogAsync(string id);
        Task<IEnumerable<Blog>> GetBlogsByDurationAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<BlogCreationDto>> GetAllBlogsAsync();

    }
}
