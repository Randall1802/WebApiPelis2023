using AutoMapper;
using WebApiPelis2023.DTOs;
using WebApiPelis2023.Models;

namespace WebApiPelis2023.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Opinion, OpinionDTO>(); // Crear un mapeo entre Opinion y OpinionDTO
            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(dest => dest.Generos, opt => opt.MapFrom(src => src.Generos.Select(g => g.Nombre).ToList()))
                .ForMember(dest => dest.Opiniones, opt => opt.MapFrom(src => src.Opiniones)); // Mapear las opiniones
            CreateMap<Genero, GeneroDTO>().ForMember(dest => dest.Peliculas, 
                opt => opt.MapFrom(src => src.Peliculas.Select(g => g.Titulo).ToList()));
        }
    }
}
