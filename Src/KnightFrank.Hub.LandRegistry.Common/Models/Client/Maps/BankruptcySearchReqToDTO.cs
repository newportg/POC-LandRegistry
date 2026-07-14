using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Maps
{
    public class BankruptcySearchReqToDTO : Profile
    {
        public BankruptcySearchReqToDTO()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<BankruptcySearchReq, LandRegistryDto>()
                .ForMember(dst => dst.RequestType, opt => opt.MapFrom(src => src.RequestType));
        }
    }
}
