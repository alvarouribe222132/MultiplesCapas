using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionITM.AppMovil.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace GestionITM.AppMovil.ViewModels
{
	// Regla de oro 1: La clase debe ser partial y heredadr de ObservableObject
		public partial class ProfesoresViewModel : ObservableObject
	{
		private readonly IHttpClientFactory _httpClientFactory;
		// Regla de oro 2: Usar ObservableCollection, NUNCA List.
		// Una lista normal no le avisa a la pantalla cuando se agrega un dato.
		//ObservableCollection<ProfesorModel> ListaProfesores {get; set;}
		public ObservableCollection<ProfesorModel> ListaProfesores { get; set; } = new ObservableCollection<ProfesorModel>(); //para llamar al boton ListaProfesores.Clear()

		// Regla de oro 3:  Las variables que cambian en pantalla van en MINÚSCULAS
		// y llevan el atributo [ObservableProperty]
		[ObservableProperty]
		private string tituloPantalla = "Directorio de profesores ITM";

		[ObservableProperty]
		private bool estaCargando;

		public ProfesoresViewModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
			ListaProfesores = new ObservableCollection<ProfesorModel>();

			// para cargar automaticamente la lista de profesores
			//CargarProfesoresCommand.Execute(null);
			_ = CargarProfesoresAsync();
		}

		// Regla de oro 4:  Los botones no llaman métodos normales , llaman "Comandos".
		// El atributo [RelayCommand] convierte este método en un botón conectable desde la vista.
		[RelayCommand]
		public async Task CargarProfesoresAsync()
		{
			if (EstaCargando) return; // Evita que el usuario hunda el botón 50 veces seguidas.

			EstaCargando = true; // Al cambiar a true, la pantalla va a mostra una ruedita de carga.

			try
			{
				var cliente = _httpClientFactory.CreateClient("GestionITMApi");
				//Intentamos traer los datos reales
				var datos = await cliente.GetFromJsonAsync<List<ProfesorModel>>("Profesor");

				// Simulamos una demora de red (Ir a buscar a la API)
				await Task.Delay(2000);

				// Agregamos datos falsos por el momento
				ListaProfesores.Clear();
				foreach (var p in datos)
				{
					ListaProfesores.Add(p);
				}
			}
			catch (Exception ex)

			{
				Console.WriteLine($"Error al cargar profesores: {ex.Message}");
			}

			finally
			{
				EstaCargando = false; // Apagamos la ruedita de carga
			}
		}
	}
}
