using Microsoft.Extensions.Logging;
using GestionITM.AppMovil.Handlers; // Asegúrate de tener el namespace correcto para AuthHandler
using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder.UseMauiApp<App>().ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			}		
			);

			// Registrar el Handler
			builder.Services.AddTransient<AuthHandler>();

			// Configurar el HttpClient con el Handler
			builder.Services.AddHttpClient("GestionITMApi", client =>
			{
				// OJO: Si usas Android Emulator, localhost es 10.0.2.2
				client.BaseAddress = new Uri("https://10.0.2.2:7123/api/");
			})
			.AddHttpMessageHandler<AuthHandler>();

			// PAGINAS
			builder.Services.AddTransient<LoginView>();
			builder.Services.AddTransient<CatalogoView>();

			return builder.Build();
		}
	}
}
