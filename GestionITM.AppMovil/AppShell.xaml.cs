using GestionITM.AppMovil.Views;

namespace GestionITM.AppMovil
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute("catalogo", typeof(CatalogoView));
			Routing.RegisterRoute("login", typeof(LoginView));
			//MainPage = new AppShell();		
		}
	}
}
