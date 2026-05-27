using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;

namespace GestionITM.Api.Mappings
{
	//Configura los Mapeos entreentidades y DTOs
	public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
		//para Estudiantes y Profesores, conect
			CreateMap<Estudiante, EstudianteDto>() //las siguentes 2 loneas se colocan par corregir el problem del mapeo de los ID de los estudiantes
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EstudianteId))
			.ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
			.ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento));

			CreateMap<EstudianteCreateDto, Estudiante>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre)); // ← conectar Nombre → Name

			CreateMap<EstudianteUpdateDto, Estudiante>()  // ← para conectar los nombres dispares
			.ForMember(dest => dest.EstudianteId, opt => opt.MapFrom(src => src.EstudianteId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre))
			.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));

			//para profesores
			CreateMap<Profesor, ProfesorDto>()
			.ForMember(dest => dest.NombreCompleto,	opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
			.ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento));

			CreateMap<ProfesorCreateDto, Profesor>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NombreCompleto));

			CreateMap<ProfesorUpdateDto, Profesor>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NombreCompleto))
			.ForMember(dest => dest.Especialidad, opt => opt.MapFrom(src => src.Especialidad))
			.ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Documento))
			.ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))
			.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));

			//para matriculas
			CreateMap<Matricula, MatriculaDto>()
			.ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso != null ? src.Curso.Nombre : "N/A"))
			.ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante != null ? src.Estudiante.Name : "N/A"))
			//AQUI SE AGREGO != null ? src.Estudiante.Name : "Estudiante eliminado" DEBIDO A QUE HACIENDO PRUEBAS HABIA ELIMINADO REGISTROS QUE EN LA BD estaban pero en la App no se mostraban
			.ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
			.ForMember(dest => dest.Periodo, opt => opt.MapFrom(src => src.Periodo))
			.ForMember(dest => dest.CursoId, opt => opt.MapFrom(src => src.CursoId))
			.ForMember(dest => dest.EstudianteId, opt => opt.MapFrom(src => src.EstudianteId));

			CreateMap<MatriculaCreateDto, Matricula>();


			CreateMap<MatriculaUpdateDto, Matricula>();
			//.ForMember(dest => dest.Estudiante, opt => opt.MapFrom(src => src.NombreEstudiante));


			//para cursos 
			CreateMap<Curso, CursoDto>()
			.ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Nombre)) //esto con el fin de que cuando se haga un Get se muestre el nombre del curso (CursoDto = Curso.cs )
			.ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.Profesor != null ? src.Profesor.Name : "Sin Profesor"))
			.ForMember(dest => dest.CuposDisponibles, opt => opt.MapFrom(src => src.CuposDisponibles))
			.ForMember(dest => dest.ProfesorId, opt => opt.MapFrom(src => src.ProfesorId));

			CreateMap<CursoCreateDto, Curso>()
			.ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.NombreCurso));

			CreateMap<CursoUpdateDto, Curso>()
			.ForMember(dest => dest.Nombre,  opt => opt.MapFrom(src => src.Nombre))
			.ForMember(dest => dest.CuposDisponibles,opt => opt.MapFrom(src => src.CuposDisponibles));

		}
	}
}
