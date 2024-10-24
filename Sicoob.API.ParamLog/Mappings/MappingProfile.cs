using AutoMapper;
using Acelera.API.ParamLog.DTO;
using Acelera.API.ParamLog.Model;

namespace Acelera.API.ParamLog.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ParametrizacaoDTO, Parametrizacao>()
            .ForMember(dest => dest.IDPARAMETRIZACAO, opt => opt.Ignore())
            .ForMember(dest => dest.TIPOCARGA, opt => opt.Ignore())
            .ForMember(dest => dest.CODCRIADOPOR, opt => opt.Ignore())
            .ForMember(dest => dest.DATAHORACRIACAO, opt => opt.Ignore())
            .ForMember(dest => dest.CODALTERADOPOR, opt => opt.Ignore())
            .ForMember(dest => dest.DATAHORAALTERACAO, opt => opt.Ignore());
        }
    }
}
