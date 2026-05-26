using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.Views;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace GestionITM.AppMovil.Views;

public partial class LoginView : ContentPage
{
	private readonly IHttpClientFactory _httpClientFactory;

	public LoginView(IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
		_httpClientFactory = httpClientFactory;

		//_httpClientFactory =
		//Application.Current!
		//.Handler!
		//.MauiContext!
		//.Services
		//.GetRequiredService<IHttpClientFactory>();
	}
	private async void OnLoginClicked(object sender, EventArgs e)
	{
		try
		{
			var loginData = new
			{
				Email = EmailEntry.Text,
				Password = PassEntry.Text
			};

			var client = _httpClientFactory.CreateClient("GestionITMApi");

			var response = await client.PostAsJsonAsync(
					"auth/login",
					loginData);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
				if (result == null || string.IsNullOrEmpty(result.Token))
				{
					await DisplayAlertAsync("Error", "Token inválido recibido", "OK");
					return;
				}

				await SecureStorage.SetAsync("auth_token", result!.Token);
				Application.Current.MainPage = new AppShell();
			}
			else
			{
				await DisplayAlertAsync("Error", "Credenciales inválidas", "OK");
			}
		}
		catch (Exception ex)
		{
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				await DisplayAlertAsync("Error", ex.Message, "OK");
			});
		}
	}
}
