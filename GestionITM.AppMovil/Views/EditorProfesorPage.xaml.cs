using GestionITM.AppMovil.Models;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views;


public partial class EditorProfesorPage : ContentPage
{
	private readonly ProfesorModel _profesor;
	private readonly IHttpClientFactory _httpClientFactory;

	public EditorProfesorPage(ProfesorModel profesor,IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();

		_profesor = profesor;
		_httpClientFactory = httpClientFactory;

		EntryNombre.Text = profesor.NombreCompleto;
		EntryCorreo.Text = profesor.Correo;
		EntryDatoExtra.Text = profesor.Especialidad;
	}

	private async void OnGuardarClicked(object sender, EventArgs e)
	{
		try
		{
			var client =
				_httpClientFactory.CreateClient("GestionITMApi");

			var response = await client.PutAsJsonAsync(
				$"Profesor/{_profesor.ProfesorId}",
				new
				{
					ProfesorId = _profesor.ProfesorId,
					NombreCompleto = EntryNombre.Text,
					Correo = EntryCorreo.Text,
					Especialidad = EntryDatoExtra.Text,
					Documento = _profesor.Documento
				});

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync(
					"Éxito",
					"Profesor actualizado",
					"OK");

				await Navigation.PopModalAsync();
			}
			else
			{
				await DisplayAlertAsync(
					"Error",
					"No se pudo actualizar",
					"OK");
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