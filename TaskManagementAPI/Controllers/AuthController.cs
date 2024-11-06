using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtServices _jwtService;
        public AuthController(UserManager<User> userManager, JwtServices jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new User { UserName = dto.Username, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { Errors = errors }); // Return a clearer error message
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });

        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInDto googleSignInDto)
        {
            try
            {
                // Verify the Google ID token
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleSignInDto.IdToken);

                //Check if the user already exists
                var user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    // Create a new user if the user does not exist
                    user = new User
                    {
                        Email = payload.Email,
                        UserName = payload.Name
                    };
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return BadRequest(new { Errors = errors });
                    }
                }
                // Generate a JWT token
                var token = _jwtService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (InvalidJwtException)
            {
                // The token is invalid
                return Unauthorized(new { Error = "Invalid Google token." });
            }
        }
    }
}
