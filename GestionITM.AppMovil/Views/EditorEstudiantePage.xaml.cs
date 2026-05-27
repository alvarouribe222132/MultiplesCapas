
using System.Net.Http.Json;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.Views;


public partial class EditorEstudiantePage : ContentPage
{
	private readonly EstudianteDto _estudiante;
	private readonly IHttpClientFactory _httpClientFactory;

	public EditorEstudiantePage(EstudianteDto estudiante,
								 IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
		_estudiante = estudiante;
		_httpClientFactory = httpClientFactory;

		EntryNombre.Text = estudiante.NombreCompleto;
		EntryCorreo.Text = estudiante.Correo;
		EntryTelefono.Text = estudiante.Telefono;
		EntryDocumento.Text = estudiante.Documento;

	}

	private async void OnGuardarClicked(object sender, EventArgs e)
	{
		if (!EntryCorreo.Text.EndsWith("@correo.itm.edu.co"))
		{
			await DisplayAlertAsync("Correo inválido",
				"Debes usar @correo.itm.edu.co", "OK");
			return;
		}

		try
		{
			var client = _httpClientFactory.CreateClient("GestionITMApi");

			var response = await client.PutAsJsonAsync(
				$"Estudiante/{_estudiante.Id}",
				new
				{
					EstudianteId = _estudiante.Id,
					Nombre = EntryNombre.Text,
					Correo = EntryCorreo.Text,
					Telefono = EntryTelefono.Text ?? "",
					Documento = EntryDocumento.Text ?? ""
				});

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync("Éxito", "Estudiante actualizado", "OK");
				await Navigation.PopModalAsync();
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

	private async void OnCancelarClicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}