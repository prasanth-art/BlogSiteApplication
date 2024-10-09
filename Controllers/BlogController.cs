using BlogSiteApplication.DTO;
using BlogSiteApplication.Models;
using BlogSiteApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSiteApplication.Controllers
{
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogCreationDto blogDto)
        {
            if (ModelState.IsValid)
            {
                var blog = new Blog
                {
                    BlogName = blogDto.BlogName,
                    Category = blogDto.Category,
                    Article = blogDto.Article,
                    AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    AuthorName = blogDto.AuthorName,
                    TimeStamp = DateTime.Now
                };

                await _blogRepository.AddBlogAsync(blog);

                return Ok(new { Message = "Blog created successfully" });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetBlogsByCategory(string category)
        {
            var blog = await _blogRepository.GetBlogsByCategoryAsync(category);
            if (blog == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(blog);
        }

        [Authorize]
        [HttpGet("user/getAllBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogRepository.GetAllBlogsAsync();
            if (blogs == null)
                return BadRequest(ModelState);
            return Ok(blogs);
        }   

        [Authorize]
        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetBlogsByUser(string userName)
        {
            var blogs = await _blogRepository.GetBlogsByUserAsync(userName); 
            if (blogs == null)
                return BadRequest(ModelState);
            return Ok(blogs);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(string id)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            await _blogRepository.DeleteBlogAsync(id);

            return Ok(new { Message = "Blog deleted successfully" });
        }

        [Authorize]
        [HttpGet("duration")]
        public async Task<IActionResult> GetBlogByDuration([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var blogs = await _blogRepository.GetBlogsByDurationAsync(startDate, endDate);
            if (blogs == null)
                return BadRequest(ModelState);
            return Ok(blogs);
        }
    }
}
