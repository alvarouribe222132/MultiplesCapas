using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.AppMovil.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

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
