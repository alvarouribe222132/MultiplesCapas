using GestionITM.AppMovil.Models;
using System.Net.Http.Json;


namespace GestionITM.AppMovil.Views;

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
				"https://10.0.2.2:7123/api/auth/login",
				loginData);

			if (response.IsSuccessStatusCode)
			{
				var result =
					await response.Content.ReadFromJsonAsync<LoginResponse>();

				await SecureStorage.SetAsync(
					"auth_token",
					result!.Token);

				await Shell.Current.GoToAsync("//catalogo");
			}
			else
			{
				await DisplayAlert(
					"Error",
					"Credenciales inválidas",
					"OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert(
				"Error",
				ex.Message,
				"OK");
		}
	}
}