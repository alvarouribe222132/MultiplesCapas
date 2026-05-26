using GestionITM.AppMovil.Views;
using GestionITM.AppMovil.ViewModels;
using Microsoft.Extensions.Logging;
//using Android.Webkit;
//using Android.App;
using GestionITM.AppMovil.Handlers;

namespace GestionITM.AppMovil;
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

		// REGISTRAR HANDLER JWT
		builder.Services.AddTransient<AuthHandler>();
		builder.Services.AddTransient<LoginView>();
		// HTTP CLIENT
		builder.Services.AddHttpClient("GestionITMApi", client =>
			{
				client.BaseAddress =new Uri("http://10.0.2.2:5144/api/");
				client.Timeout = TimeSpan.FromSeconds(30);
			}).AddHttpMessageHandler<AuthHandler>();

		builder.Services.AddTransient<ProfesoresViewModel>();

		// ===  INYECCIÓN DE DEPENDENCIAS ===
		// Usamos AddTransient para que cada vez  que entremos a la pantalla,
		// nazca una versión fresca y limpia de la vista y de su cerebro (ViewModel).

		builder.Services.AddTransient<ProfesoresPage>();
		builder.Services.AddTransient<CatalogoView>();
		builder.Services.AddTransient<EstudiantesPage>();
		builder.Services.AddTransient<MatriculasPage>();

		builder.Logging.AddDebug();
		

		return builder.Build();
		}
	}