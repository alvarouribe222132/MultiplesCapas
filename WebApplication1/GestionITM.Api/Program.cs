using AutoMapper;
using GestionITM.Api.Mappings; // agregado para usar ApplicationDbContext
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure;
using GestionITM.Infrastructure.Services;
using GestionITM.Infrastructure.Repositorios;
using GestionITM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


/*aqui le decimos al programa como ensamblar las piezas como si fueran piesas le lego
 */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

//Registrar los Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//1. configuramos la cadena de conexion a la base de datos, en este caso se esta usando SQL Server,
//pero se puede usar cualquier otra base de datos que sea compatible con Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	//el AddDbContext registra nuestra seccion de BD en el sistema central de .NET
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
		?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
/*con el UseSqlServer definimos nuestro motor de BD, En un posible Futuro donde querramos cambiar a otro motor
*de BD como MySQL, PostgreSQL, etc. solo tendriamos que cambiar esta linea de codigo.
*y el resto del codigo seguiria funcionando sin problemas
*el GetConnectionString("DefaultConnection") se utiliza por seguridad
*NOTA: las rutas de BD nunca se escriben directamente en el codigo, 
*Se escriben en el archivo appsettings.json 
*Entonces el GetConnectionString es una instruccion que va a obtener la ruta del .JSON que se guardo con el nombre DefaultConnection
*
*/

/*2. Registramos el repositorio de estudiante para la inyeccion de dependencias, esto nos permite usar el repositorio en el controlador sin tener que preocuparnos por la instanciacion del mismo
*AddScoped se usa para crear una instancia del repositorio por cada solicitud HTTP, esto es importante para evitar problemas de concurrencia y para asegurar que cada solicitud tenga su propia instancia del repositorio
*Basicamente le estamos diciendo a .NET que siempre que un controlador le pida a la interfaz IEstudianteRepository, le entregue una instancia de la clase EstudianteRepository
*Tú automaticamente estregale la instancia lista para usar la clase real que es en este caso EstudianteRepository
*
*/
builder.Services.AddScoped<InterfaceEstudRepositorio, EstudianteRepository>();
builder.Services.AddScoped<IEstudianteService, EstudianteServices>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
//Registrar ApplicationDbContext

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

app.UseHttpsRedirection();
app.UseAuthorization();

