using GestionITM.AppMovil.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views;

public partial class CatalogoView : ContentPage
{
	private readonly IHttpClientFactory _httpClientFactory;

	ObservableCollection<CursoDto> ListaCursos =
		new ObservableCollection<CursoDto>();

	int paginaActual = 1;

	bool cargando = false;

	public CatalogoView(IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();

		_httpClientFactory = httpClientFactory;

		CursosCollection.ItemsSource = ListaCursos;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (ListaCursos.Count == 0)
		{
			await CargarCursos();
		}
	}

	async Task CargarCursos()
	{
		if (cargando)
			return;

		cargando = true;

		try
		{
			var client =
				_httpClientFactory.CreateClient("GestionITMApi");

			var result =
				await client.GetFromJsonAsync<PagedResults<CursoDto>>(
					$"curso/paginado?Pagina={paginaActual}&RegistrosPorPagina=10");

			if (result != null)
			{
				foreach (var curso in result.Items)
				{
					ListaCursos.Add(curso);
				}

				paginaActual++;
			}
		}
		finally
		{
			cargando = false;
		}
	}

	private async void OnRemainingItemsThresholdReached(
		object sender,
		EventArgs e)
	{
		await CargarCursos();
	}

	private async void OnMatricularClicked(
		object sender,
		EventArgs e)
	{
		var button = sender as Button;

		var curso = button?.BindingContext as CursoDto;

		if (curso == null)
			return;

		await MatricularEstudiante(curso.IdCurso);
	}

	public async Task MatricularEstudiante(int cursoId)
	{
		try
		{
			var client =
				_httpClientFactory.CreateClient("GestionITMApi");

			var response =
				await client.PostAsJsonAsync(
					"matricula",
					new
					{
						IdCurso = cursoId
					});

			if (response.IsSuccessStatusCode)
			{
				await DisplayAlert(
					"Éxito",
					"Matrícula realizada",
					"Genial");
			}
			else if (response.StatusCode ==
					 System.Net.HttpStatusCode.BadRequest)
			{
				var errorContent =
					await response.Content.ReadAsStringAsync();

				await DisplayAlert(
					"Aviso",
					errorContent,
					"Entendido");
			}
			else
			{
				await DisplayAlert(
					"Error",
					"Algo salió mal",
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