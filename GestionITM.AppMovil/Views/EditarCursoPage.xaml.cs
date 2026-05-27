using GestionITM.AppMovil.Models;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views;

public partial class EditarCursoPage : ContentPage
{
	private readonly CursoDto _curso;
	private readonly IHttpClientFactory _httpClientFactory;

	public EditarCursoPage(
		CursoDto curso,
		IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();

		_curso = curso;
		_httpClientFactory = httpClientFactory;

		// Cargar datos en pantalla
		txtNombre.Text = _curso.NombreCurso;
		txtCodigo.Text = _curso.Codigo;
		txtCreditos.Text = _curso.Creditos.ToString();
		txtCupos.Text = _curso.CuposDisponibles.ToString();
	}

	private async void OnGuardarClicked(object sender, EventArgs e)
	{
		try
		{
			// Validar nombre
			if (string.IsNullOrWhiteSpace(txtNombre.Text))
			{
				await DisplayAlertAsync(
					"Error",
					"El nombre es obligatorio",
					"OK");

				return;
			}

			// Validar números
			if (!int.TryParse(txtCreditos.Text, out int creditos) ||
				!int.TryParse(txtCupos.Text, out int cupos))
			{
				await DisplayAlertAsync(
					"Error",
					"Créditos y cupos deben ser números",
					"OK");

				return;
			}

			// Validar rango
			if (creditos <= 0 || creditos > 30)
			{
				await DisplayAlertAsync(
					"Error",
					"Los créditos deben estar entre 1 y 30",
					"OK");

				return;
			}

			if (cupos <= 0)
			{
				await DisplayAlertAsync(
					"Error",
					"Los cupos deben ser mayores a cero",
					"OK");

				return;
			}

			var client = _httpClientFactory
				.CreateClient("GestionITMApi");

			// DTO correcto para el backend
			var updateDto = new
			{
				IdCurso = _curso.IdCurso,
				NombreCurso = txtNombre.Text,
				Codigo = txtCodigo.Text,
				Creditos = creditos,
				CuposDisponibles = cupos,
				ProfesorId = _curso.ProfesorId,
				Estado = _curso.Estado
			};

			var response = await client.PutAsJsonAsync(
				$"Curso/{_curso.IdCurso}",
				updateDto);

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync(
					"Éxito",
					"Curso actualizado correctamente",
					"OK");

				await Navigation.PopAsync();
			}
			else
			{
				var error = await response.Content.ReadAsStringAsync();

				await DisplayAlertAsync(
					"Error",
					$"No se pudo actualizar el curso:\n{error}",
					"OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync(
				"Error",
				$"Ocurrió un error: {ex.Message}",
				"OK");
		}
	}

	private async void OnCancelarClicked(
		object sender,
		EventArgs e)
	{
		await Navigation.PopAsync();
	}
}