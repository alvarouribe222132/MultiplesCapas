using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionITM.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost("login")]
		public IActionResult Login()
		{
			// Claims del usuario
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, "estudianteITM"),

                new Claim(ClaimTypes.Role, "Estudiante")
			};

			// Llave secreta
			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(
					_configuration["Jwt:Key"]!));

			var creds = new SigningCredentials(
				key,
				SecurityAlgorithms.HmacSha256);

			// Crear token
			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(2),
				signingCredentials: creds
			);

			// Retornar token
			return Ok(new
			{
				token = new JwtSecurityTokenHandler().WriteToken(token)
			});
		}
	}
}