using AutoMapper;
using LaTiendaApi.Models;
using LaTiendaApi.DTOs;

namespace LaTiendaApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaNombre,
                           opt => opt.MapFrom(src => src.objCategoria != null
                                                     ? src.objCategoria.Nombre
                                                     : string.Empty));

            CreateMap<ProductoCreateDto, Producto>();

            CreateMap<Categoria, CategoriaDto>();
        }
    }
}