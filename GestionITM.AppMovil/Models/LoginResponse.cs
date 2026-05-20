using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.AppMovil.Models
{
	public class LoginResponse
	{
		public string Token { get; set; } = string.Empty;
		public string Expiration { get; set; } = string.Empty;
	}	
}
