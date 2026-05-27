using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;

namespace GestionITM.AppMovil.Views
{
    public partial class CatalogoView : ContentPage
    {
        private readonly IHttpClientFactory _httpClientFactory;

		public CatalogoView() : this(App.Current!.Handler!.MauiContext!.Services.GetRequiredService<IHttpClientFactory>())
		{
		}

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

		private async void OnNuevoCursoClicked(object sender, EventArgs? e)
		{
			string nombre = await DisplayPromptAsync("Nuevo Curso", "Nombre del curso:");
			if (string.IsNullOrWhiteSpace(nombre)) return;

			string creditosStr = await DisplayPromptAsync("Nuevo Curso",
				"Créditos:", keyboard: Keyboard.Numeric);
			if (!int.TryParse(creditosStr, out int creditos)) return;

			string cuposStr = await DisplayPromptAsync("Nuevo Curso",
				"Cupos disponibles:", keyboard: Keyboard.Numeric);
			if (!int.TryParse(cuposStr, out int cupos)) return;

			string codigo = await DisplayPromptAsync("Nuevo Curso", "Código del curso:");
			if (string.IsNullOrWhiteSpace(codigo)) return;

			// Abrir selector de profesor
			//ProfesorModel? profesorSeleccionado = null;


			// ABRIR SELECTOR
			var selectorPage = new SeleccionProfesorPage(_httpClientFactory);


			
			//selectorPage.ProfesorSeleccionado += profesor =>
			//{
			//	profesorSeleccionado = profesor;
			//};

			await Navigation.PushModalAsync(selectorPage);
			var profesorSeleccionado =	await selectorPage.EsperarSeleccionAsync();

			// Esperar a que el usuario seleccione un profesor
			//await Task.Delay(500);


			if (profesorSeleccionado == null)
			{
				await DisplayAlertAsync("Aviso",
					"Debes seleccionar un profesor para continuar", "OK");
				return;
			}

			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var body = new
				{
					NombreCurso = nombre,
					Creditos = creditos,
					CuposDisponibles = cupos,
					Codigo = codigo,
					ProfesorId = profesorSeleccionado.ProfesorId
				};

				var response = await client.PostAsJsonAsync("Curso", body);
				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync("Éxito",
						$"Curso creado con profesor {profesorSeleccionado.NombreCompleto}", "OK");
					ListaCursos.Clear();
					paginaActual = 1;
					await CargarCursos();
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
                else {
				//para leer el mensaje del backend
					var errorContent = await response.Content.ReadAsStringAsync();

					// Intentar extraer el mensaje del JSON
					try
					{
						var errorObj = System.Text.Json.JsonDocument.Parse(errorContent);
						var mensaje = errorObj.RootElement
							.GetProperty("message").GetString();
						await DisplayAlertAsync("Aviso", mensaje ?? errorContent, "Entendido");
					}
					catch
					{
						await DisplayAlertAsync("Aviso", errorContent, "Entendido");
					}
				}
			}
            catch (Exception ex)
            {
                // CORRECCIÓN: igual que arriba, DisplayAlert directo evita las advertencias.
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

		private async void OnEditarCursoClicked(object sender, EventArgs e)
		{
			var curso = (sender as Button)?.BindingContext as CursoDto;
			if (curso == null) return;
			// para abrirías una página de edición 
			await DisplayAlertAsync("Editar", $"Editar curso: {curso.NombreCurso}", "OK");
		}

		private async void OnInactivarCursoClicked(object sender, EventArgs e)
		{
			var curso = (sender as Button)?.BindingContext as CursoDto;

			if (curso == null)
				return;

			bool confirmar = await DisplayAlertAsync(
				"Confirmar",
				"¿Desea inactivar este curso?",
				"Sí",
				"No");

			if (!confirmar)
				return;

			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");

				// Enviar actualización al backend
				var body = new
				{
					IdCurso = curso.IdCurso,
					NombreCurso = curso.NombreCurso,
					Creditos = curso.Creditos,
					CuposDisponibles = curso.CuposDisponibles,
					Codigo = curso.Codigo,
					NombreProfesor = curso.NombreProfesor,
					ProfesorId = curso.ProfesorId,
					Estado = "Inactivo"
				};

				var response = await client.PutAsJsonAsync(
					$"Curso/{curso.IdCurso}",
					body);

				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync(
						"Éxito",
						"Curso inactivado correctamente",
						"OK");

					// Recargar lista
					ListaCursos.Clear();
					paginaActual = 1;

					await CargarCursos();
				}
				else
				{
					var error = await response.Content.ReadAsStringAsync();

					await DisplayAlertAsync(
						"Error",
						error,
						"OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync(
					"Error",
					ex.Message,
					"OK");
			}
		}

		private async void OnRemainingItemsThresholdReached(
			object sender,
			EventArgs? e)
		{
			await CargarCursos();
		}

	}
    
}