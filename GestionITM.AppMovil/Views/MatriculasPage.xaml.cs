using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.AppMovil.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace GestionITM.AppMovil.Views
{
	public partial class MatriculasPage : ContentPage
	{
		private readonly IHttpClientFactory _httpClientFactory;
		public ObservableCollection<MatriculaDto> ListaMatriculas { get; set; } = new();

		public MatriculasPage(IHttpClientFactory httpClientFactory)
		{
			InitializeComponent();
			_httpClientFactory = httpClientFactory;
			BindingContext = this;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await CargarMatriculas();
		}

		async Task CargarMatriculas()
		{
			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var lista = await client.GetFromJsonAsync<List<MatriculaDto>>("Matricula");
				if (lista != null)
				{
					ListaMatriculas.Clear();
					foreach (var m in lista)
						ListaMatriculas.Add(m);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Error", ex.Message, "OK");
			}
		}
	}

}
