using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.ViewModels
{
	public class EstudiantesViewModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		public ObservableCollection<EstudianteDto> Estudiantes { get; set; } = new();

		public EstudiantesViewModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task CargarEstudiantesAsync()
		{
			try
			{
				var client = _httpClientFactory.CreateClient("GestionITMApi");
				var lista = await client.GetFromJsonAsync<List<EstudianteDto>>("estudiante");

				if (lista != null)
				{
					Estudiantes.Clear();
					foreach (var e in lista)
					{
						Estudiantes.Add(e);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
