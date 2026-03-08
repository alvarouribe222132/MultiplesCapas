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
			CreateMap<Estudiante, EstudianteDto>();
			CreateMap<EstudianteCreateDto, Estudiante>();
		
		}
	}
}
