using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volxyseat.Domain.Models.InvalidTokenModel;
using Volxyseat.Domain.ViewModel;
using Volxyseat.Infrastructure.Configurations;

namespace Volxyseat.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTConfig _jwtConfig;
        private readonly IConfiguration _configuration;
        private List<InvalidToken> _invalidTokens = new List<InvalidToken>();

        public AuthenticationController
            (
            UserManager<IdentityUser> userManager,
            JWTConfig jwtConfig,
            IConfiguration configuration,
            List<InvalidToken> invalidTokens
            )
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig;
            _configuration = configuration;
            _invalidTokens = invalidTokens;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] ClientViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newUser = new IdentityUser()
            {
                Email = request.Email,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(newUser);
                return Ok(new { token });
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginRequest)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (existingUser == null)
                {
                    return BadRequest();
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

                if (!isCorrect)
                {
                    return BadRequest();
                }

                return Ok();
            }

            return BadRequest();
        }

        

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
 
            return Ok(users);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
            _invalidTokens.Add(new InvalidToken { TokenId = token });
            return Ok(new { message = "Logout bem-seucedido" });
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                }),

                Expires = DateTime.Now.ToUniversalTime().AddHours(2),
                NotBefore = DateTime.Now.ToUniversalTime(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
