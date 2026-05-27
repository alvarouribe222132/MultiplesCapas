using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;

namespace GestionITM.AppMovil.Views
{
	public partial class MatriculasPage : ContentPage
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly MatriculasViewModel _viewModel;

		public MatriculasPage(MatriculasViewModel viewModel, IHttpClientFactory httpClientFactory)
		{
			InitializeComponent();
			_viewModel = viewModel;
			_httpClientFactory = httpClientFactory;
			BindingContext = _viewModel; // Aquí conectamos la interfaz con los datos

		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			// Llamamos al método que está en el ViewModel
			await _viewModel.CargarMatriculasAsync();
		}
		private async void OnMatriculaTapped(object sender, EventArgs e)
		{
			var matricula = (sender as Border)?.BindingContext as MatriculaDto;
			if (matricula == null) return;
		
			string? accion = await this.DisplayActionSheet("Opciones de Matrícula", "Cancelar", null, "Inactivar Matrícula", "Ver Detalle");
			if (string.IsNullOrEmpty(accion) || accion == "Cancelar")
				return;

			if (accion == "Inactivar Matrícula")
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				// se Envia un objeto con el estado cambiado
				var response = await client.PutAsJsonAsync($"Matricula/{matricula.Id}", new
				{
					MatriculaId = matricula.Id,
					Estado = "Inactiva"
				});

				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync("Éxito", "Matrícula inactivada", "OK");
					await _viewModel.CargarMatriculasAsync(); // Refrescar lista
				}
			}
		}
		private async void OnAgregarMatriculaClicked(object sender, EventArgs e)
		{
			// Abrimos la nueva página como un Modal (ventana emergente completa)
			await Navigation.PushModalAsync(new NuevaMatriculaPage(_httpClientFactory));
		}

	}

}
