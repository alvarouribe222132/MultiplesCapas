using GestionITM.AppMovil.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;

namespace GestionITM.AppMovil.Views
{
	public partial class EstudiantesPage : ContentPage
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private ObservableCollection<EstudianteDto> ListaEstudiantes = new();

		public EstudiantesPage(IHttpClientFactory httpClientFactory)
		{
			InitializeComponent();
			_httpClientFactory = httpClientFactory;
			EstudiantesCollection.ItemsSource = ListaEstudiantes;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await CargarEstudiantes();
		}
		async Task CargarEstudiantes()
		{
			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var lista = await client.GetFromJsonAsync<List<EstudianteDto>>("Estudiante");
				if (lista != null)
				{
					ListaEstudiantes.Clear();
					foreach (var e in lista)
						ListaEstudiantes.Add(e);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Error", ex.Message, "OK");
			}
		}
		private async void OnEditarClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			var estudiante = button?.BindingContext as EstudianteDto;
			if (estudiante == null) return;

			// usando el helper que creamos para pedir el nombre
			string nuevoNombre = await DisplayPromptAsync("Editar Estudiante", "Nombre completo:", estudiante.NombreCompleto) ?? "";
			if (string.IsNullOrWhiteSpace(nuevoNombre)) return;

			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");

				// se Crea el objeto con los datos actualizados
				var body = new
				{
					Id = estudiante.Id,
					Nombre = nuevoNombre,
					Telefono = estudiante.Telefono
				};

				var response = await client.PutAsJsonAsync($"estudiante/{estudiante.Id}", body);

				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync("Éxito", "Estudiante actualizado correctamente", "OK");
					// Llamar al método del ViewModel para refrescar la lista
					await CargarEstudiantes();
				}
				else
				{
					await DisplayAlertAsync("Error", "No se pudo actualizar el estudiante", "OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Error", ex.Message, "OK");
			}
		}

		private async void OnEliminarClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			var estudiante = button?.BindingContext as EstudianteDto;
			if (estudiante == null) return;

			bool confirmar = await DisplayAlertAsync("Eliminar", $"¿Estás seguro de eliminar a {estudiante.NombreCompleto}?", "Sí", "No");
			if (!confirmar) return;

			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var response = await client.DeleteAsync($"estudiante/{estudiante.Id}");

				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync("Éxito", "Estudiante eliminado", "OK");
					await CargarEstudiantes();
				}
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Error", ex.Message, "OK");
			}

		}
		private async void OnAgregarClicked(object sender, EventArgs e)
		{
			string nombre = await DisplayPromptAsync("Nuevo Estudiante", "Nombre completo:");
			if (string.IsNullOrWhiteSpace(nombre)) return;

			string correo = ""; //para validar correo antes de continuar
			while (true)
			{
				correo = await DisplayPromptAsync("Nuevo Estudiante",
					"Correo institucional (@correo.itm.edu.co):");
				if (string.IsNullOrWhiteSpace(correo)) return;

				if (correo.EndsWith("@correo.itm.edu.co")) break;

				await DisplayAlertAsync("Correo inválido",
					"Debes usar un correo @correo.itm.edu.co", "Intentar de nuevo");
			}

			string telefono = await DisplayPromptAsync("Nuevo Estudiante", "Teléfono:");
			string documento = await DisplayPromptAsync("Nuevo Estudiante", "Documento:");

			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var body = new
				{
					Nombre = nombre,
					Correo = correo,
					Telefono = telefono,
					Documento = documento
				};

				var response = await client.PostAsJsonAsync("Estudiante", body);
				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync("Éxito", "Estudiante registrado", "OK");
					await CargarEstudiantes();
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
}
