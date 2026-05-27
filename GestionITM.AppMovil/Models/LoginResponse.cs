using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GestionITM.AppMovil.Models
{
	public class LoginResponse
	{

		[JsonPropertyName("token")]
		public string Token { get; set; } = string.Empty;

		[JsonPropertyName("expiration")]
		public string Expiration { get; set; } = string.Empty;
	}	
}
