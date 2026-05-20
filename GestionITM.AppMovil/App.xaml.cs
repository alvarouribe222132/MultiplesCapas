using Microsoft.Extensions.DependencyInjection;

namespace GestionITM.AppMovil
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
		}
	}
}