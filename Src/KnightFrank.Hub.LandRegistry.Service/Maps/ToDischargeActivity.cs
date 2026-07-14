using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToDischargeActivity : Profile
    {
        public ToDischargeActivity() 
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestDischargeActivityV1_0Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType4>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType3>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.SubjectProperty, opt => opt.MapFrom(src => src));

            CreateMap<Identity, Q1ExternalReferenceType6>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Identity, Q1CustomerReferenceType1>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<Request, Q1SubjectPropertyType1>()
                .ForMember(dst => dst.ContinueIfTitleIsClosedAndContinuedIndicator, opt => opt.MapFrom(src => src.Flags.ClosedAndContinued))
                .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.Property.TitleNumber))
                .ForMember(dst => dst.ChargeDate, opt => opt.MapFrom(src => src.Property.ChargeDate));

            CreateMap<string, Q2TextType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType2>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<DateTime, DateType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.ToString("yyyy-MM-dd")));

        }
    }
}
