using System.ComponentModel.DataAnnotations;

namespace BlogSiteApplication.DTO
{
    public class BlogCreationDto
    {
        public string? Id { get; set; }
        [Required]
        [MinLength(20)]
        public string? BlogName { get; set; }

        [Required]
        [MinLength(20)]
        public string? Category { get; set;}

        [Required]
        [MinLength(20)]
        public string? Article { get; set;}

        [Required]
        public string? AuthorName { get; set;}
    }
}
