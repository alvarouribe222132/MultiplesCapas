using GestionITM.AppMovil.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views;

public partial class SeleccionProfesorPage : ContentPage
{
	private readonly IHttpClientFactory _httpClientFactory;
	private List<ProfesorModel> _todosLosProfesores = new();
	private ObservableCollection<ProfesorModel> _profesoresFiltrados = new();

	// Este evento le avisa a CatalogoView qué profesor eligió el usuario
	public event Action<ProfesorModel>? ProfesorSeleccionado;

	public SeleccionProfesorPage(IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
		_httpClientFactory = httpClientFactory;
		ProfesoresCollection.ItemsSource = _profesoresFiltrados;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await CargarProfesores();
	}

	async Task CargarProfesores()
	{
		try
		{
			var client = _httpClientFactory.CreateClient("GestionITMApi");
			var lista = await client.GetFromJsonAsync<List<ProfesorModel>>("Profesor");
			if (lista != null)
			{
				_todosLosProfesores = lista;
				_profesoresFiltrados.Clear();
				foreach (var p in lista)
					_profesoresFiltrados.Add(p);
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", ex.Message, "OK");
		}
	}

	private void OnBuscarTextChanged(object sender, TextChangedEventArgs e)
	{
		var texto = e.NewTextValue?.ToLower() ?? "";

		var filtrados = _todosLosProfesores.Where(p =>
			p.NombreCompleto.ToLower().Contains(texto) ||
			p.Documento.ToLower().Contains(texto) ||
			p.Especialidad.ToLower().Contains(texto) ||
			p.ProfesorId.ToString().Contains(texto));

		_profesoresFiltrados.Clear();
		foreach (var p in filtrados)
			_profesoresFiltrados.Add(p);
	}

	private async void OnSeleccionarClicked(object sender, EventArgs e)
	{
		var button = sender as Button;
		var profesor = button?.BindingContext as ProfesorModel;
		if (profesor == null) return;

		// Avisar a quien abrió esta página qué profesor se seleccionó
		ProfesorSeleccionado?.Invoke(profesor);
		await Navigation.PopModalAsync();
	}
}