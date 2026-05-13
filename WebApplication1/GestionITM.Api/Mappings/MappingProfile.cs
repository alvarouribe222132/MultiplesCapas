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
		.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre));
			CreateMap<Profesor, ProfesorDto>();
			CreateMap<ProfesorCreateDto, Profesor>();


			CreateMap<Matricula, MatriculaDto>()
			.ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Nombre))
			
		.ForMember(dest => dest.NombreEstudiante,opt => opt.MapFrom(src => src.Estudiante.Name));


			CreateMap<MatriculaCreateDto, Matricula>();

			CreateMap<MatriculaUpdateDto, Matricula>();

			CreateMap<Matricula, MatriculaDto>()
			.ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Nombre))

			.ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Name));

		}
	}
}
