using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Models.Api;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new LoginResponseDto 
                { 
                    Success = false, 
                    Message = "Username and password are required." 
                });
            }

            if (_tokenService.ValidateCredentials(request.Username, request.Password))
            {
                var token = _tokenService.GenerateToken(request.Username);
                return Ok(new LoginResponseDto 
                { 
                    Success = true, 
                    Message = "Login successful", 
                    Token = token 
                });
            }

            return Unauthorized(new LoginResponseDto 
            { 
                Success = false, 
                Message = "Invalid username or password." 
            });
        }

#if DEBUG
        [HttpGet("demo-credentials")]
        public IActionResult GetDemoCredentials()
        {
            var credentials = new
            {
                Message = "Demo credentials for testing (only available in DEBUG builds)",
                Users = new[]
                {
                    new { Username = "admin", Password = "password123" },
                    new { Username = "user", Password = "userpass" },
                    new { Username = "api_user", Password = "api_secret" }
                }
            };

            return Ok(credentials);
        }
#endif
    }
}
