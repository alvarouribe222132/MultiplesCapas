using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;

namespace GestionITM.AppMovil.Models
{
	public class AuthHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(
				HttpRequestMessage request,
				CancellationToken cancellationToken)
		{
			var token = await SecureStorage.GetAsync("auth_token");

			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
