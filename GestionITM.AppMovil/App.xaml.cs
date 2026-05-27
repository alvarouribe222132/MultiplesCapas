using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using GestionITM.AppMovil.Views;

namespace GestionITM.AppMovil
{
	public partial class App : Application
	{
		private readonly LoginView _loginView;

		public App(LoginView loginView)
		{
			InitializeComponent();

			_loginView = loginView;
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(
				new NavigationPage(_loginView));
		}
	}
}