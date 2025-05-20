using Application.Dtos;
using Application.Dtos.User;
using Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(ITokenService tokenService, IConfiguration configuration, IAuthService authService)
        {
            _tokenService = tokenService;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            //decode access token 
            var payload = await ValidateGoogleToken(request.AccessToken);

            if (payload.EmailVerified == false)
            {
                return BadRequest("Email not verified");
            }

            var response = await _authService.LoginAsync(payload, request);

            return Ok(response);
        }

        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["Authentication:Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch
            {
                return null;
            }
        }
    }
}
