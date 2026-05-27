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
		private readonly MatriculasViewModel _viewModel;

		public MatriculasPage(MatriculasViewModel viewModel)
		{
			InitializeComponent();
			_viewModel = viewModel;
			BindingContext = _viewModel; // Aquí conectamos la interfaz con los datos
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			// Llamamos al método que está en el ViewModel
			await _viewModel.CargarMatriculasAsync();
		}

	}

}
