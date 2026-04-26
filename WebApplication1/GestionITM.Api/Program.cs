using GestionITM.Api.Mappings; // agregado para usar ApplicationDbContext
using GestionITM.Domain.Interfaces;
using GestionITM.Api.Middleware;
using GestionITM.Infrastructure.Services;
using GestionITM.Infrastructure.Repositorios;
using GestionITM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; //Para usar Encoding.UTF8.GetBytes
using System.Reflection;
using Microsoft.OpenApi; //Necesario para Assembly.GetExecutingAssembly() en la configuracion del Swagger

/*aqui le decimos al programa como ensamblar las piezas como si fueran piesas le lego
 */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

//Registrar los Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo 
	{ 
		Title = "GestionITM API", 
		Version = "v1" 
		
	});

	//Instruccion Nueva
	//Localice el archivo xml generado en la carpeta de binario (bin) despues de compilar el proyecto. El nombre del archivo suele ser el mismo que el nombre del proyecto, seguido de .xml (por ejemplo, GestionITM.API.xml)

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

	//Le dice al Swagger que incluya los comentarios XML para mejorar la documentacion de la API. Esto es especialmente util para descubrir los endpoints, parametros y respuestas.
	c.IncludeXmlComments(xmlPath);
});

//1. configuramos la cadena de conexion a la base de datos, en este caso se esta usando SQL Server,
//pero se puede usar cualquier otra base de datos que sea compatible con Entity Framework Core


builder.Services.AddDbContext<ApplicationDbContext>(options =>
	//el AddDbContext registra nuestra seccion de BD en el sistema central de .NET
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
		//?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
/*con el UseSqlServer definimos nuestro motor de BD, En un posible Futuro donde querramos cambiar a otro motor
*de BD como MySQL, PostgreSQL, etc. solo tendriamos que cambiar esta linea de codigo.
*y el resto del codigo seguiria funcionando sin problemas
*el GetConnectionString("DefaultConnection") se utiliza por seguridad
*NOTA: las rutas de BD nunca se escriben directamente en el codigo, 
*Se escriben en el archivo appsettings.json 
*Entonces el GetConnectionString es una instruccion que va a obtener la ruta del .JSON que se guardo con el nombre DefaultConnection
*
*/

//configuracion de JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options => { 		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],

			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
		};
	});

/*2. Registramos el repositorio de estudiante para la inyeccion de dependencias, esto nos permite usar el repositorio en el controlador sin tener que preocuparnos por la instanciacion del mismo
*AddScoped se usa para crear una instancia del repositorio por cada solicitud HTTP, esto es importante para evitar problemas de concurrencia y para asegurar que cada solicitud tenga su propia instancia del repositorio
*Basicamente le estamos diciendo a .NET que siempre que un controlador le pida a la interfaz IEstudianteRepository, le entregue una instancia de la clase EstudianteRepository
*Tú automaticamente estregale la instancia lista para usar la clase real que es en este caso EstudianteRepository
*
*/
builder.Services.AddScoped<InterfaceEstudRepositorio, EstudianteRepository>();
builder.Services.AddScoped<IEstudianteService, EstudianteServices>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();

//AddScoped crea una instancia para cada solicitud HTTP, pero usa la misma instancia en las otras llamadas dentro de la misma petición web
builder.Services.AddScoped<IProfesorService, ProfesorServices>();
builder.Services.AddScoped<InterfaceProfeRepositorio, ProfesorRepository>();
//Registrar ApplicationDbContext

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleWare>(); //Agregamos el middleware de excepciones para manejar los errores de manera centralizada (ESCUDO DE PROTECCION CONTRA ERRORES)
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//Bloque de activiacion de autenticacion y autorizacion, esto es importante para proteger las rutas de nuestra API y asegurar que solo los usuarios autorizados puedan acceder a ellas
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

