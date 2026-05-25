using GestionITM.Domain.Dtos;
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
		public IActionResult Login([FromBody] LoginRequestDto loginDto)
		{
			// Validación de credenciales
			if (string.IsNullOrEmpty(loginDto.Email) ||
			string.IsNullOrEmpty(loginDto.Password))
				return BadRequest("Email y contraseña son requeridos");

			//aceptamos un  correo @correo.itm.edu.co
			if (!loginDto.Email.EndsWith("@correo.itm.edu.co"))
				return Unauthorized("Solo usuarios ITM pueden ingresar");

			var claims = new[]
			{
		new Claim(ClaimTypes.Name, loginDto.Email),
		new Claim(ClaimTypes.Email, loginDto.Email),
		new Claim(ClaimTypes.Role, "Estudiante"),
		new Claim("EstudianteId", "1") // viene de la BD
    };

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(2),
				signingCredentials: creds);

			return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
		}
	}
}