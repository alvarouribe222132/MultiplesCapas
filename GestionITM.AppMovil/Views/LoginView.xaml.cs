using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.AppMovil.Models;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views
{
	public partial class LoginView : ContentPage
	{
		public LoginView()
		{
			InitializeComponent();
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

				var client = new HttpClient();

				var response = await client.PostAsJsonAsync(
					"http://10.0.2.2:7123/api/auth/login",
					loginData);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
					if (result == null || string.IsNullOrEmpty(result.Token))
					{
						await DisplayAlertAsync("Error", "Token inválido recibido", "OK");
						return;
					}

					await SecureStorage.SetAsync("auth_token",result!.Token);
					await Shell.Current.GoToAsync("//catalogo");
				}
				else
				{
					await DisplayAlertAsync("Error","Credenciales inválidas","OK");
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

}