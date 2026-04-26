using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GestionITM.Domain.Dtos;
using Xunit; //la plantilla que estamos usando es del framework de pruebas unitarias xUnit, esta nos permite escribir pruebas para nuestro código y verificar que funcione correctamente.	
using Moq;  // Moq libreria que crea los moqs los dobles de accion los encargados de simular el comportamiento de las dependencias en nuestras pruebas unitarias, esto nos permite aislar la lógica que queremos probar y evitar efectos secundarios no deseados.
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure.Services;
using Domain.Entities;

namespace GestionITM.Tests
{
	public class ProfesorServiceTest
	//prueba 1: el camino  triste (validar que falle cuando debe de fallar)
	[Fact] //Este atributo le indica a visual studio que este metodo es una prueba unitaria

	public async Task RegistrarProfesorConSpecialityVacia_DebeLanzarExcepcion()
	{
		//1 Arrange (preparar el escenario de prueba)

		//2 creamos las dobles acciones (Mocks) usando nuestras interfaces

		//3 Esta es la razon  por la que creamos InterfaceprofeRepositorio: para poder simular su comportamiento sin necesidad de tener una base de datos real o una implementación concreta, esto nos permite aislar la lógica que queremos probar y evitar efectos secundarios no deseados.
		var mockRepository = new Mock<InterfaceProfeRepositorio>();
		var mockUserService = new Mock<IUserService>();   //el profesor lo tiene como new Mock<IcurrentUserService>()

		//le decimos al mock del usuario como debe actuar si le preguntan por el ID del usuario actual
		mockUserService.Setup(x => x.ObtenerEmailUsuario().Returns("admin@itm.edu.co");

		//Inistanciamos el servicio REAL, pero le inyectamos los mocks en lugar de las implementaciones reales
		var profesorService = new ProfesorServices(mockRepository.Object, mockUserService.Object);

		//preparamos unos datos errados a proposito para probar la validacion

		var dtomalo = new ProfesorCreateDto
		{
			NombreCompleto = "Profesor Prueba",
			Email = "Prueba@itm.edu.co",
			Especialidad = "",
			Documento = "123456789",
			Correo = "
		}

		//2 Act (ejecutar la acción que queremos probar)
		// Exigimos (Assert) que al ejecutar (Act) el metodo, el sistema DEBE lanzar un ERROR
		// Si lanza el Error, la Prueba es EXITOSA (quiere decir que la seguridad si esta funcionando)
		// Si No lanza Error, La Prueba FALLA ( quiere decir que la seguridad esta rota)
await Assert.ThowsAsync<Exception>(() => profesorService.RegistrarProfesorAsync(dtomalo));
	}

	//prueba 2 : El camino feliz (validar que funcione correctamente cuando se le dan datos correctos)
	[Fact]
	public async Task RegistrarProfesorDataCorrect_DebeLLamarAlRepositorio()
	{
		//1 Arrange (preparar el escenario de prueba)
		var mockRepository = new Mock<InterfaceProfeRepositorio>();
		var mockUserService = new Mock<ICurrentUserService>();

		mocUserService.Setup(x => x.ObtenerEmailUsuario()).Returns("admin@itm.edu.co");

		var ProfesorService = new ProfesorServices(mockRepository.Object, mockUserService.Object);
		var dtocorrecto = new ProfesorCreateDto
		{
			NombreCompleto = "Profesor Prueba",
			Email = "Prueba@itm.edu.co#,
			Especialidad = "Arquitectura",

		//el profesor lo tiene como new Mock<IcurrentUserService>()
		//2 creamos las dobles acciones (Mocks) usando nuestras interfaces
		//3 Esta es la razon  por la que creamos InterfaceprofeRepositorio: para poder simular su comportamiento sin necesidad de tener una base de datos real o una implementación concreta, esto nos permite aislar la lógica que queremos probar y evitar efectos secundarios no deseados.
		var mockRepository = new Mock<InterfaceProfeRepositorio>();
		var mockUserService = new Mock<IUserService>();   //el profesor lo tiene como new Mock<IcurrentUserService>()
		//le decimos al mock del usuario como debe actuar si le preguntan por el ID del usuario actual
		mockUserService.Setup(x => x.ObtenerEmailUsuario().Returns("

	//creamos 
}