using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.AppMovil.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views
{
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
                var client = _httpClientFactory.CreateClient("GestionITMApi");

                PagedResults<CursoDto>? result =
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

		private async void OnNuevoCursoClicked(object sender, EventArgs e)
		{
			await DisplayAlertAsync("Nuevo Curso","Abrir formulario de creación","OK");
		}
		private async void OnRemainingItemsThresholdReached(object? sender, EventArgs e)
        {
            await CargarCursos();
        }

        private async void OnMatricularClicked(object? sender, EventArgs e)
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
                var client = _httpClientFactory.CreateClient("GestionITMApi");

				var body = new
				{
					EstudianteId = 1, // de prueba por el momento, luego se obtiene del contexto de usuario
					CursoId = cursoId,
					Periodo = "2025-1"
				};

				var response = await client.PostAsJsonAsync("Matricula", body);

				if (response.IsSuccessStatusCode)
                {
                    await DisplayAlertAsync("Éxito", "Matrícula realizada", "Genial");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlertAsync("Aviso", errorContent, "Entendido");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // CORRECCIÓN: DisplayAlert se puede llamar directamente desde
                    // ContentPage en cualquier contexto. MainThread.InvokeOnMainThreadAsync
                    // es innecesario aquí y genera advertencias.
                    await DisplayAlertAsync("Error", "Sesión expirada. Inicia sesión nuevamente", "OK");
					await Shell.Current.GoToAsync("//login");
				}
                else 
                {
					await DisplayAlertAsync("Error", "Algo salió mal. Intenta nuevamente", "OK");
				}
            }
            catch (Exception ex)
            {
                // CORRECCIÓN: igual que arriba, DisplayAlert directo evita las advertencias.
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }

        }
    }
}