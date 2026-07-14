using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Maps
{
    public class OfficialSearchPartReqToDTO : Profile
    {
        public OfficialSearchPartReqToDTO()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<OfficialSearchPartReq, LandRegistryDto>()
                .ForMember(dst => dst.RequestType, opt => opt.MapFrom(src => src.RequestType))
                .ForMember(dst => dst.Request, opt => opt.MapFrom(src => src));

            CreateMap<OfficialSearchPartReq, KnightFrank.Hub.LandRegistry.Common.Models.Request>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.Identity))
                .ForMember(dst => dst.Property, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.PrioritySearch, opt => opt.MapFrom(src => src.PrioritySearch));

            CreateMap<KnightFrank.Hub.LandRegistry.Common.Models.Client.Identity, KnightFrank.Hub.LandRegistry.Common.Models.Identity>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueMsgId))
                .ForMember(dst => dst.ExternalRef, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.CustomerRef, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<OfficialSearchPartReq_Property, Property>()
                .ForMember(dst => dst.ExpectedPrice, opt => opt.MapFrom(src => src.ExpectedPrice))
                .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.TitleNumber));

            CreateMap<OfficialSearchPartReq_PrioritySearch, PrioritySearch>()
                .ForMember(dst => dst.PriorityType, opt => opt.MapFrom(src => src.PriorityType))
                .ForMember(dst => dst.SearchFrom, opt => opt.MapFrom(src => src.SearchFrom))
                .ForMember(dst => dst.PropreietorOrFirstApplicant, opt => opt.MapFrom(src => src.PropreietorOrFirstApplicant))
                .ForMember(dst => dst.ApplicantNames, opt => opt.MapFrom(src => src.ApplicantNames))
                .ForMember(dst => dst.PropertyIdentification, opt => opt.MapFrom(src => src.PropertyIdentification))
                .ForMember(dst => dst.ContinueIfFeeExceeds, opt => opt.MapFrom(src => src.ContinueIfFeeExceeds))
                .ForMember(dst => dst.ContinueIfNameMismatch, opt => opt.MapFrom(src => src.ContinueIfNameMismatch))
                .ForMember(dst => dst.TimeshareDetails, opt => opt.MapFrom(src => src.TimeshareDetails));

            CreateMap<OfficialSearchPartReq_PropertyIdentification, PropertyIdentification>()
                .ForMember(dst => dst.PropertyDescription, opt => opt.MapFrom(src => src.PropertyDescription))
                .ForMember(dst => dst.EstatePlan, opt => opt.MapFrom(src => src.EstatePlan))
                .ForMember(dst => dst.TitlePlan, opt => opt.MapFrom(src => src.TitlePlan))
                .ForMember(dst => dst.PlanAttachment, opt => opt.MapFrom(src => src.PlanAttachment));

            CreateMap<OfficialSearchPartReq_TitlePlan, TitlePlan>()
                .ForMember(dst => dst.TitlePlanNumber, opt => opt.MapFrom(src => src.TitlePlanNumber))
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.Reference));

            CreateMap<OfficialSearchPartReq_EstatePlan, EstatePlan>()
                .ForMember(dst => dst.ApprovalDate, opt => opt.MapFrom(src => src.ApprovalDate))
                .ForMember(dst => dst.PlotDetails, opt => opt.MapFrom(src => src.PlotDetails));

            CreateMap<OfficialSearchPartReq_PlanAttachment, PlanAttachment>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.CopyrightNotices, opt => opt.MapFrom(src => src.CopyrightNotices))
                .ForMember(dst => dst.BinaryObject, opt => opt.MapFrom(src => src.BinaryObject));

            CreateMap<OfficialSearchPartReq_TimeshareDetails, TimeshareDetails>()
                .ForMember(dst => dst.TimeshareLease, opt => opt.MapFrom(src => src.TimeshareLease))
                .ForMember(dst => dst.TimePeriod, opt => opt.MapFrom(src => src.TimePeriod));


        }
    }
}
