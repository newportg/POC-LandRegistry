using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Maps
{
    public class SearchByPropertyDescriptionReqToDTO : Profile
    {
        public SearchByPropertyDescriptionReqToDTO()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<SearchByPropertyDescriptionReq, LandRegistryDto>()
                .ForMember(dst => dst.RequestType, opt => opt.MapFrom(src => src.RequestType))
                .ForMember(dst => dst.Request, opt => opt.MapFrom(src => src));

            CreateMap<SearchByPropertyDescriptionReq, KnightFrank.Hub.LandRegistry.Common.Models.Request>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.Identity))
                .ForMember(dst => dst.Property, opt => opt.MapFrom(src => src.Property));

            CreateMap<KnightFrank.Hub.LandRegistry.Common.Models.Client.Identity, KnightFrank.Hub.LandRegistry.Common.Models.Identity>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueMsgId))
                .ForMember(dst => dst.ExternalRef, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.CustomerRef, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<SearchByPropertyDescriptionReq_Property, Property>()
                .ForMember(dst => dst.PropertyNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dst => dst.PropertyName, opt => opt.MapFrom(src => src.BuildingName))
                .ForMember(dst => dst.Line1, opt => opt.MapFrom(src => src.StreetName))
                .ForMember(dst => dst.City, opt => opt.MapFrom(src => src.CityName))
                .ForMember(dst => dst.PostCode, opt => opt.MapFrom(src => src.PostcodeZone));
        }
    }
}
