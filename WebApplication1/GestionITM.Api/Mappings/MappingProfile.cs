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
			CreateMap<Estudiante, EstudianteDto>() //las siguentes 2 loneas se colocan par corregir el problem del mapeo de los ID de los estudiantes
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EstudianteId))
			.ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Name));
			CreateMap<EstudianteCreateDto, Estudiante>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre)); // ← conectar Nombre → Name

			CreateMap<EstudianteUpdateDto, Estudiante>()  // ← para conectar los nombres dispares
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre))
			.ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));

			CreateMap<Profesor, ProfesorDto>()
			.ForMember(dest => dest.NombreCompleto,	opt => opt.MapFrom(src => src.Name));
			CreateMap<ProfesorCreateDto, Profesor>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NombreCompleto));


			CreateMap<Matricula, MatriculaDto>()
			.ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Nombre))
			.ForMember(dest => dest.NombreEstudiante,opt => opt.MapFrom(src => src.Estudiante.Name));


			CreateMap<MatriculaCreateDto, Matricula>();

			CreateMap<MatriculaUpdateDto, Matricula>();


			CreateMap<Curso, CursoDto>()
			.ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Nombre)) //esto con el fin de que cuando se haga un Get se muestre el nombre del curso (CursoDto = Curso.cs )
			.ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.Profesor != null ? src.Profesor.Name : "Sin Profesor"))
			.ForMember(dest => dest.CuposDisponibles, opt => opt.MapFrom(src => src.CuposDisponibles));


			CreateMap<CursoCreateDto, Curso>()
			.ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.NombreCurso));

			CreateMap<CursoUpdateDto, Curso>()
			.ForMember(dest => dest.Nombre,  opt => opt.MapFrom(src => src.Nombre));
		}
	}
}
