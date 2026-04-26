using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Dtos;
using GestionITM.Infrastructure.Services;
using AutoMapper;
using GestionITM.Domain.Entities;
using Microsoft.Identity.Client;

namespace GestionITM.Test
{
	public class ProfesorServiceTest
	{
		//prueba 1: El camino triste (validar que falle cuando debe de fallar)
		[Fact] //este atributo le indica a visual studio que este metodo es una prueba unitaria
		public async Task RegistrarProfesorConSpecialityVacia_DebeLanzarExcepcion()
		{
			//1 Arrange (preparar el escenario de prueba)

			//2 creamos las dobles acciones (Mocks) usando nuestras interfaces

			//3 Esta es la razon  por la que creamos InterfaceprofeRepositorio: para poder simular su comportamiento sin necesidad de tener una base de datos real o una implementación concreta, esto nos permite aislar la lógica que queremos probar y evitar efectos secundarios no deseados.
			var mockRepository = new Mock<InterfaceProfeRepositorio>();
			var mockMapper = new Mock<IMapper>();

			//Configuramos el mapper para devolver un Profesor cualquiera cuando reciba un ProfesorCreateDto

			//le decimos al mock del usuario como debe actuar si le preguntan por el ID del usuario actual
			mockMapper
				.Setup(m => m.Map<Profesor>(It.IsAny<ProfesorCreateDto>()))
				.Returns(new Profesor());

			// Instanciamos el servicio REAL, pero le inyectamos los mocks en ligar de las implementaciones reales
			var profesorService = new ProfesorServices(mockRepository.Object, mockMapper.Object);

			var dtomalo = new ProfesorCreateDto
			{
			//profesor de prueba
				NombreCompleto = "JuanJulano Perez",
				Especialidad = "", //especialidad vacia, esto es lo que queremos probar
				Documento = "123456789",
				Correo = "JuanJulano.perez@itm.edu.co"

			};

			//2. Act (Ejecutar la acción que queremos probar)

			//Exigimos (Assert) que al ejecutar (Act) el metodo, el sistema Debe lanzar un error.
			//Si lanza el error, LA PRUEBA FALLA (Porque nuestra seguridad funcionó)
			//Si No lanza el error, LA PRUEBA FALLA (Porque nuestra seguridad esta rota)
			var resultado = await profesorService.RegistrarProfesorAsync(dtomalo);
			Assert.False(resultado);
		}
		//prueba 2: El camino feliz (validar que funcione cuando debe de funcionar)
		[Fact]
		public async Task RegistrarProfesor_DatosCorrectos_DebellamarAlRepositorio()
		{
			//1. Arrange
			var mockRepository = new Mock<InterfaceProfeRepositorio>();
			var mockMapper = new Mock<IMapper>();

			mockMapper
			.Setup(m => m.Map<Profesor>(It.IsAny<ProfesorCreateDto>()))
			.Returns(new Profesor());

			var profesorService = new ProfesorServices(mockRepository.Object, mockMapper.Object);
			var dtobien = new ProfesorCreateDto
			{
				NombreCompleto = "Ana",
				Especialidad = "Arquitectura",
				Documento = "123456789",
				Correo = "ana@itm.edu.co"

			};

			//2. Act
			await profesorService.RegistrarProfesorAsync(dtobien);

			//3. Assert
			//Verificamos que el metodo AgregarAsync del repositorio fue llamado exactamente una vez
			//It.IsAny<Profesor>()  significa que no nos importa el objeto Profesor especifico que se le pasó, solo queremos asegurarnos
			mockRepository.Verify(x => x.AgregarAsync(It.IsAny<Profesor>()), Times.Once());

		}
	}
}

