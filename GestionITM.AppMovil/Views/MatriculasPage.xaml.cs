using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.ViewModels;
using System;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views
{
	public partial class MatriculasPage : ContentPage
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly MatriculasViewModel _viewModel;

		public MatriculasPage(
			MatriculasViewModel viewModel,
			IHttpClientFactory httpClientFactory)
		{
			InitializeComponent();

			_viewModel = viewModel;
			_httpClientFactory = httpClientFactory;

			BindingContext = _viewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await _viewModel.CargarMatriculasAsync();
		}

		// TAP SOBRE TODA LA TARJETA
		private async void OnMatriculaTapped(object sender, EventArgs e)
		{
			var matricula =
				(sender as Border)?.BindingContext as MatriculaDto;

			if (matricula == null)
				return;

			string? accion = await DisplayActionSheetAsync("Opciones de Matrícula","Cancelar",null,"Inactivar Matrícula","Ver Detalle");

			if (string.IsNullOrEmpty(accion) ||	accion == "Cancelar")
			return;

			if (accion == "Ver Detalle")
			{
				await DisplayAlertAsync(	"Detalle",$"""Estudiante: {matricula.NombreEstudiante} Curso: {matricula.NombreCurso} Estado: {matricula.Estado} Periodo: {matricula.Periodo} """, "OK");

				return;
			}

			if (accion == "Inactivar Matrícula")
			{
				await InactivarMatriculaAsync(matricula);
			}
		}

		// BOTÓN inactivar dentro de la tarjeta
		private async void OnInactivarMatriculaClicked(
			object? sender,
			EventArgs e)
		{
			var matricula =
				(sender as Button)?.BindingContext as MatriculaDto;

			if (matricula == null)
				return;

			await InactivarMatriculaAsync(matricula);
		}

		// BOTÓN Editar
		private async void OnEditarMatriculaClicked(object? sender,EventArgs e)
		{
			var matricula =	(sender as Button)?.BindingContext as MatriculaDto;

			if (matricula == null)
				return;

			await Navigation.PushAsync(new EditarMatriculaPage(matricula, _httpClientFactory));
			//await DisplayAlertAsync("Editar",$"Editar matrícula de {matricula.NombreEstudiante}","OK");

			// Aquí luego puedes abrir una página:
			// await Navigation.PushAsync(new EditarMatriculaPage(...));
		}

		// LÓGICA CENTRALIZADA
		private async Task InactivarMatriculaAsync(
			MatriculaDto matricula)
		{
			bool confirmar = await DisplayAlertAsync("Confirmar",$"¿Desea inactivar la matrícula de {matricula.NombreEstudiante}?",	"Sí","No");

			if (!confirmar)
				return;

			try
			{
				var client =
					_httpClientFactory.CreateClient("GestionITMApi");

				var body = new
				{
					MatriculaId = matricula.Id,
					Estado = "Inactiva"
				};

				var response =
					await client.PutAsJsonAsync(
						$"Matricula/{matricula.Id}",
						body);

				if (response.IsSuccessStatusCode)
				{
					await DisplayAlertAsync("Éxito","Matrícula inactivada correctamente","OK");

					await _viewModel.CargarMatriculasAsync();
				}
				else
				{
					var error =
						await response.Content.ReadAsStringAsync();

					await DisplayAlertAsync("Error",error,"OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Error",ex.Message,"OK");
			}
		}

		// BOTÓN NUEVA MATRÍCULA
		private async void OnAgregarMatriculaClicked(object sender,	EventArgs e)
		{
			await Navigation.PushModalAsync(new NuevaMatriculaPage(_httpClientFactory));
		}
	}
}