using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;

namespace GestionITM.AppMovil.Handlers
{
	public class AuthHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// 1. Intentar obtener el token del almacenamiento seguro del celular
			var token = await SecureStorage.GetAsync("auth_token");

			if (!string.IsNullOrEmpty(token))
			{
				// 2. Inyectarlo en el Header
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
