using System.ComponentModel.DataAnnotations;

namespace BlogSiteApplication.DTO
{
    public class UserRegistrationDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@".+@.+\.com$", ErrorMessage = "Email must be a valid email address with '@' and end with '.com'.")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Password must contain 8 characters in Alphanumeric form.")]
        public string? Password { get; set; }
    }
}
