using GestionITM.AppMovil.Models;
using GestionITM.Domain.Dtos;
using System.Net.Http.Json;
using CursoDto = GestionITM.AppMovil.Models.CursoDto;

namespace GestionITM.AppMovil.Views;

public partial class EditarCursoPage : ContentPage
{
	private readonly CursoDto _curso;
	private readonly IHttpClientFactory _httpClientFactory;

	// Recibimos el curso seleccionado y el factory para la API
	public EditarCursoPage(CursoDto curso, IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
		_curso = curso;
		_httpClientFactory = httpClientFactory;

		// Cargamos los datos en la interfaz
		txtNombre.Text = _curso.NombreCurso;
		txtCodigo.Text = _curso.Codigo;
		txtCreditos.Text = _curso.Creditos.ToString();
		txtCupos.Text = _curso.CuposDisponibles.ToString();
	}

	private async void OnGuardarClicked(object sender, EventArgs e)
	{

		if (!int.TryParse(txtCreditos.Text, out int creditos) ||
				!int.TryParse(txtCupos.Text, out int cupos))
		{
			await DisplayAlertAsync("Error", "Créditos y cupos deben ser números", "OK");
			return;
		}
		var updateDto = new
		{
			IdCurso = _curso.IdCurso,
			Nombre = txtNombre.Text,
			Codigo = txtCodigo.Text,
			Creditos = creditos,
			CuposDisponibles = cupos,
			ProfesorId = _curso.ProfesorId,
			//Estado = "Activo"
		};
		try
		{

			var client = _httpClientFactory.CreateClient("GestionITMApi");
			// Nota: Asegúrate de que la URL coincida con tu Controller (ver punto 4)
			var response = await client.PutAsJsonAsync($"Curso/{_curso.IdCurso}", updateDto);

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync("Éxito", "Curso actualizado correctamente", "OK");
				await Navigation.PopAsync(); // Regresar a la lista
			}
			else
			{
			var error = await response.Content.ReadAsStringAsync();
				await DisplayAlertAsync("Error", $"No se pudo actualizar el curso: {error}", "OK");
			}
		}
		catch(Exception ex){
  			await DisplayAlertAsync("Error", $"Ocurrió un error: {ex.Message}", "OK");
		}
		}

		
	private async void OnCancelarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}