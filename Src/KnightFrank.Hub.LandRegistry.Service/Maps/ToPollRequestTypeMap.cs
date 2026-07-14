using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToPollRequestTypeMap : Profile
    {
        public ToPollRequestTypeMap()
        {
            // AutoMapper doesnt do nested or Custom to Custom mapping :(
            CreateMap<LandRegistryDto, PollRequestType>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, MessageIDTextType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));
        }
    }
}
