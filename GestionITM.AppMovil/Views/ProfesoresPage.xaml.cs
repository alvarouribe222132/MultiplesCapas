using GestionITM.AppMovil.Models;
using GestionITM.AppMovil.ViewModels;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views
{
	public partial class ProfesoresPage : ContentPage
	{
		// Inyectamos el ViewModel por Constructor, para que la pantalla tenga acceso a los datos y comandos.
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ProfesoresViewModel _viewModel;
		public ProfesoresPage(ProfesoresViewModel viewModel, IHttpClientFactory httpClientFactory)
		{

			InitializeComponent();
			_viewModel = viewModel;
			_httpClientFactory = httpClientFactory;

			// ESTO ES EL CORDON UMBILICAL ENTRE LA PANTALLA Y EL VIEWMODEL.  Sin esto, la pantalla no sabe de dónde sacar los datos.
			// Le decimos a la página gráfica que su cerebro será este ViewModel que le inyectamos por constructor.
			BindingContext = viewModel;

		}
		// --- HELPERS DE INTERFAZ ---
		private Task<string?> Prompt(string titulo, string mensaje, string? valorInicial = null)
			=> MainThread.InvokeOnMainThreadAsync(() => DisplayPromptAsync(titulo, mensaje, initialValue: valorInicial ?? ""));

		private Task<bool> Confirmar(string titulo, string mensaje, string botonSi = "Sí", string botonNo = "No")
			=> MainThread.InvokeOnMainThreadAsync(() => DisplayAlert(titulo, mensaje, botonSi, botonNo));

		private Task Alerta(string titulo, string mensaje, string v)
			=> MainThread.InvokeOnMainThreadAsync(() => DisplayAlert(titulo, mensaje, "OK"));
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await _viewModel.CargarProfesoresAsync();
		}

		private async void OnVerDetalleClicked(object sender, EventArgs e)
		{
			var profesor = (sender as BindableObject)?.BindingContext as ProfesorModel;
			//aqui Usamos el operador ? para evitar que truene si es nulo
			if (profesor == null) return;

			await Alerta("Ver Detalles",
				$"👤 Nombre: {profesor.NombreCompleto}\n" +
				$"📧 Correo: {profesor.Correo ?? "No asignado"}\n" +
				$"🎓 Especialidad: {profesor.Especialidad ?? "N/A"}\n" +
				$"🪪 Documento: {profesor.Documento ?? "Sin Datos"}",
				"Cerrar");
		}

		private async void OnAgregarClicked(object sender, EventArgs e)
		{
			string nombre = await Prompt("Nuevo Profesor", "Nombre completo:") ?? "";
			if (string.IsNullOrWhiteSpace(nombre)) return;

			string correo = await Prompt("Nuevo Profesor", "Correo (@correo.itm.edu.co):") ?? "";
			if (!correo.EndsWith("@correo.itm.edu.co"))
			{
				await Alerta("Correo inválido", "Debes usar @correo.itm.edu.co", "Intentar de nuevo");
				return;
			}

			string especialidad = await Prompt("Nuevo Profesor", "Especialidad:") ?? "";
			string documento = await Prompt("Nuevo Profesor", "Documento:") ?? "";
			string? telefono = await Prompt("Nuevo Profesor", "Teléfono (opcional):");


			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var response = await client.PostAsJsonAsync("Profesor", new
				{
					NombreCompleto = nombre,
					Correo = correo,
					Especialidad = especialidad,
					Documento = documento,
					Telefono = telefono ?? ""
				});

				if (response.IsSuccessStatusCode)
				{
					await Alerta("Éxito", "Profesor registrado", "OK");
					await _viewModel.CargarProfesoresAsync();
				}
			}

			catch (Exception ex)
			{
				await Alerta("Error", ex.Message, "OK");
			}
		}

		private async void OnEditarClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			var profesor = button?.BindingContext as ProfesorModel;
			if (profesor == null) return;

			await Navigation.PushModalAsync(new EditorProfesorPage(profesor, _httpClientFactory));
		}
		private async void OnEliminarClicked(object sender, EventArgs e)
		{
			var profesor = (sender as Button)?.BindingContext as ProfesorModel;
			if (profesor == null) return;

			if (await Confirmar("Eliminar", $"¿Desea Eliminar al profesor {profesor.NombreCompleto}?", "Sí", "No"))
			{
				try
				{
					var client = _httpClientFactory.CreateClient("GestionITMApi");
					var response = await client.DeleteAsync($"Profesor/{profesor.ProfesorId}");    //aqui muestra la linea roja sobre ProfesorId

					if (response.IsSuccessStatusCode)
					{
						await Alerta("Éxito", "Profesor eliminado", "OK");
						await _viewModel.CargarProfesoresAsync();
					}
				}
				catch (Exception ex)
				{
					await Alerta("Error", ex.Message, "OK");
				}
			}
		}
	}
}
