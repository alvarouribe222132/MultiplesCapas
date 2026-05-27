using GestionITM.AppMovil.Models;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views;

public partial class NuevaMatriculaPage : ContentPage
{
	private readonly IHttpClientFactory _httpClientFactory;

	public NuevaMatriculaPage(IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
		_httpClientFactory = httpClientFactory;
		CargarDatos();
	}

	private async void CargarDatos()
	{
		try
		{
			var client = _httpClientFactory.CreateClient("GestionITMApi");

			// Traemos estudiantes y cursos para llenar los Pickers
			var estudiantes = await client.GetFromJsonAsync<List<EstudianteDto>>("Estudiante");
			var cursos = await client.GetFromJsonAsync<List<CursoDto>>("Curso");

			MainThread.BeginInvokeOnMainThread(() =>
			{
				PickerEstudiantes.ItemsSource = estudiantes;
				PickerCursos.ItemsSource = cursos;
			});

		}
		catch (Exception ex)
		{
			// Si aquí hay un error y no hay try-catch, explota con JavaProxyThrowable
			System.Diagnostics.Debug.WriteLine(ex.Message);
		}
	}

	private async void OnRegistrarClicked(object sender, EventArgs e)
	{
		var estudiante = PickerEstudiantes.SelectedItem as EstudianteDto;
		var curso = PickerCursos.SelectedItem as CursoDto;

		if (estudiante == null || curso == null)
		{
			await DisplayAlertAsync("Error", "Seleccione estudiante y curso", "OK");
			return;
		}

		var nuevaMatricula = new
		{
			EstudianteId = estudiante.Id,
			CursoId = curso.IdCurso,
			Periodo = "2025-1"  // ← agregar Periodo
		};

		try
		{
			var client = _httpClientFactory.CreateClient("GestionITMApi");
			var response = await client.PostAsJsonAsync("Matricula", nuevaMatricula);

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync("Éxito", "Matriculado correctamente", "OK");
				await Navigation.PopModalAsync();
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				try
				{
					var errorObj = System.Text.Json.JsonDocument.Parse(errorContent);
					var mensaje = errorObj.RootElement.GetProperty("message").GetString();
					await DisplayAlertAsync("Aviso", mensaje ?? errorContent, "OK");
				}
				catch
				{
					await DisplayAlertAsync("Error", errorContent, "OK");
				}
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "OK");
		}
	}

	private async void OnCancelarClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}