using BlogSiteApplication.DTO;
using BlogSiteApplication.Models;
using BlogSiteApplication.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogSiteApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public UserController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email!);
                if (existingUser != null)
                {
                    return BadRequest("Email already registered");
                }

                var user = new User
                {
                    UserName = userDto.UserName,
                    UserEmail = userDto.Email
                };

                user.Password = _passwordHasher.HashPassword(user, userDto.Password);
                await _userRepository.AddUserAsync(user);

                return Ok(new { Message = "User registered successfully" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(loginDto.Email!);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }
                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid credentials");
                }
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return BadRequest(ModelState);
        }

        private object GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.UserEmail!),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.ObjectID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
                //new Claim(ClaimTypes.Role, string.Join(",", user.Roles!))
            };
            //var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Issuer"]));
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,//_configuration["Jwt:Issuer"],
                audience: _jwtSettings.Audience,//_configuration["Jwt: Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), signingCredentials: creds); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}