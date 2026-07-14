using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Maps
{
    public class DischargeActivityReqToDTO : Profile
    {
        public DischargeActivityReqToDTO()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<DischargeActivityReq, LandRegistryDto>()
                .ForMember(dst => dst.RequestType, opt => opt.MapFrom(src => src.RequestType));
        }
    }
}
