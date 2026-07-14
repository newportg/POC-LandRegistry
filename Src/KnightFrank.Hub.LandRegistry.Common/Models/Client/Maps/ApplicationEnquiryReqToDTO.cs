using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Maps
{
    public class ApplicationEnquiryReqToDTO : Profile
    {
        public ApplicationEnquiryReqToDTO()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<ApplicationEnquiryReq, LandRegistryDto>()
                .ForMember(dst => dst.RequestType, opt => opt.MapFrom(src => src.RequestType))
                .ForMember(dst => dst.Request, opt => opt.MapFrom(src => src));

            CreateMap<ApplicationEnquiryReq, KnightFrank.Hub.LandRegistry.Common.Models.Request>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.Identity))
                .ForMember(dst => dst.Property, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.Flags, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.ApplicationReference, opt => opt.MapFrom(src => src));

            CreateMap<KnightFrank.Hub.LandRegistry.Common.Models.Client.Identity, KnightFrank.Hub.LandRegistry.Common.Models.Identity>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueMsgId))
                .ForMember(dst => dst.ExternalRef, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.CustomerRef, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<ApplicationEnquiryReq_Property, Property>()
                .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.TitleNumber));

            CreateMap<ApplicationEnquiryReq_Property, Flags>()
                .ForMember(dst => dst.ClosedAndContinued, opt => opt.MapFrom(src => src.ContinueIfTitleIsClosedAndContinuedIndicator));

            CreateMap<ApplicationEnquiryReq, ApplicationReference>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ApplicationReference));
        }
    }
}
