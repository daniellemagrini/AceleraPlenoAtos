using AutoMapper;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.DATAHORACRIACAO, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DATAHORAINATIVO, opt => opt.Ignore())
                .ForMember(dest => dest.BOLVERIFICANOMEMAQUINA, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.NUMCHECKALTERACAO, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.BOLHABILITADOUSUARIO, opt => opt.MapFrom(src => 1));

            CreateMap<UsuarioDTO, UsuarioSistema>()
                .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
                .ForMember(dest => dest.LOGIN, opt => opt.MapFrom(src => src.IDUSUARIO))
                .ForMember(dest => dest.SECRETKEY, opt => opt.Ignore())
                .ForMember(dest => dest.BOLPRIMEIROLOGIN, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.DATAHORACRIACAO, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}

