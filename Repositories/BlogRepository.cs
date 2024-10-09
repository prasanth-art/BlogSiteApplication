using BlogSiteApplication.DTO;
using BlogSiteApplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogSiteApplication.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IMongoCollection<Blog> _blogs;

        public BlogRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BlogSiteDB");
            _blogs = database.GetCollection<Blog>("Blogs");
        }
        public async Task AddBlogAsync(Blog blog)
        {
            try
            {
                await _blogs.InsertOneAsync(blog);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while adding a blog", ex);
            }
        }

        public async Task DeleteBlogAsync(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                await _blogs.DeleteOneAsync(blog => blog.Id == objectId);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured whie deleting a blog", ex);
            }
        }

        public async Task<Blog> GetBlogByIdAsync(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                return await _blogs.Find(blog => blog.Id == objectId).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while retrieving the blog by ID", ex);
            }
                    
        }

        public async Task<IEnumerable<Blog>> GetBlogsByCategoryAsync(string category)
        {
            try
            {
                return await _blogs.Find(blog => blog.Category== category).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while retrieivng blogs by category", ex);
            }
        }

        public async Task<IEnumerable<Blog>> GetBlogsByDurationAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var startDateUtc = startDate.ToUniversalTime();
                var endDateUtc = endDate.Date.AddDays(1).AddTicks(-1).ToUniversalTime();
                return await _blogs.Find(blog => blog.TimeStamp >= startDateUtc && blog.TimeStamp <= endDateUtc).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while retrieving blogs by duration", ex);
            }
        }

        public async Task<IEnumerable<Blog>> GetBlogsByUserAsync(string userName)
        {
            try
            {
                return await _blogs.Find(blog => blog.AuthorName == userName).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while retrieving blogs by user", ex);
            }
        }

        public async Task<IEnumerable<BlogCreationDto>> GetAllBlogsAsync()
        {
            try
            {
                var blogs = await _blogs.Find(_ => true).ToListAsync();

                var blogDto = blogs.Select(blog => new BlogCreationDto
                {
                    Id = blog.Id.ToString(),
                    BlogName = blog.BlogName,
                    Category = blog.Category,
                    Article = blog.Article,
                    AuthorName = blog.AuthorName
                });

                return blogDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retrieving all blogs", ex);
            }
        }
    }
}
