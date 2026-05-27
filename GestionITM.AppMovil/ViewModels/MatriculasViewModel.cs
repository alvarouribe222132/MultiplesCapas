using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.ViewModels
{
	public class MatriculasViewModel
	{
		private readonly IHttpClientFactory _httpClientFactory;

		// Esta es la lista que se mostrará en el CollectionView
		public ObservableCollection<MatriculaDto> ListaMatriculas { get; set; } = new();

		public MatriculasViewModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task CargarMatriculasAsync()
		{
			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				// Asegúrate de que la ruta coincida con tu API
				var lista = await client.GetFromJsonAsync<List<MatriculaDto>>("matricula");

				if (lista != null)
				{
					ListaMatriculas.Clear();
					foreach (var m in lista)
					{
						ListaMatriculas.Add(m);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
