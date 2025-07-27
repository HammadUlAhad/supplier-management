using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SupplierManagement.Models.Api;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Controllers
{
    /// <summary>
    /// Authentication API Controller - Exercise 2 Implementation
    /// Provides JWT-based authentication for secure API access
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v1/[controller]")] // Added v1 route support
    [Produces("application/json")]
    [EnableRateLimiting("AuthPolicy")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Exercise 2 - Authentication: Login endpoint
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT token for authenticated access</returns>
        /// <response code="200">Login successful - Returns JWT token</response>
        /// <response code="400">Bad request - Missing or invalid credentials</response>
        /// <response code="401">Unauthorized - Invalid username or password</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Exercise 2 - Authentication: Token endpoint (alias for login)
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT token for authenticated access</returns>
        /// <response code="200">Authentication successful - Returns JWT token</response>
        /// <response code="400">Bad request - Missing or invalid credentials</response>
        /// <response code="401">Unauthorized - Invalid username or password</response>
        [HttpPost("token")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status401Unauthorized)]
        public IActionResult GetToken([FromBody] LoginRequestDto request)
        {
            // Same logic as Login endpoint for compatibility
            return Login(request);
        }

#if DEBUG
        /// <summary>
        /// Get demo credentials for testing (Development only)
        /// </summary>
        /// <returns>List of demo credentials for testing purposes</returns>
        /// <response code="200">Returns demo credentials</response>
        [HttpGet("demo-credentials")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult GetDemoCredentials()
        {
            var credentials = new
            {
                Message = "Demo credentials for testing Exercise 2 APIs (only available in DEBUG builds)",
                Environment = "Development",
                Users = new[]
                {
                    new { Username = "admin", Password = "password123", Role = "Administrator" },
                    new { Username = "user", Password = "userpass", Role = "User" },
                    new { Username = "api_user", Password = "api_secret", Role = "API User" }
                },
                Instructions = new
                {
                    Step1 = "Use POST /api/auth/login or /api/auth/token to get JWT token",
                    Step2 = "Include 'Authorization: Bearer <token>' header in API requests",
                    Step3 = "Access Exercise 2 APIs: GET /api/suppliers (all suppliers with rates)",
                    Step4 = "Access Exercise 2 APIs: GET /api/suppliers/overlapping-rates (check overlaps)"
                }
            };

            return Ok(credentials);
        }
#endif
    }
}
