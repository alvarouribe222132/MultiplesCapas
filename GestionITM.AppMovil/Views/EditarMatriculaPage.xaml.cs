using GestionITM.AppMovil.Models;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views;

public partial class EditarMatriculaPage : ContentPage
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly MatriculaDto _matricula;

	public EditarMatriculaPage(
		MatriculaDto matricula,
		IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
		_matricula = matricula;
		_httpClientFactory = httpClientFactory;

		PeriodoEntry.Text = matricula.Periodo;
		EstadoPicker.SelectedItem = matricula.Estado;
	}

	private async void OnGuardarClicked(object sender, EventArgs e)
	{
		if (EstadoPicker.SelectedItem == null)
		{
			await DisplayAlertAsync("Error", "Selecciona un estado", "OK");
			return;
		}

		try
		{
			var client = _httpClientFactory.CreateClient("GestionITMApi");

			var body = new
			{
				MatriculaId = _matricula.Id,
				Periodo = PeriodoEntry.Text,
				Estado = EstadoPicker.SelectedItem.ToString(),
				NombreEstudiante = _matricula.NombreEstudiante
			};

			var response = await client.PutAsJsonAsync(
				$"Matricula/{_matricula.Id}", body);

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync("Éxito", "Matrícula actualizada", "OK");
				await Navigation.PopAsync();
			}
			else
			{
				var error = await response.Content.ReadAsStringAsync();
				await DisplayAlertAsync("Error", error, "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "OK");
		}
	}
}