using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToApplicationEnquiry : Profile
    {
        public ToApplicationEnquiry()
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestApplicationEnquiryV1_0Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType1>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => Item(src)));
                //.ForMember(dst => dst.Item, opt => opt.MapFrom(src => Property(src)));

            //CreateMap<ApplicationReference, string>()
            //    .ForMember(dst => dst, opt => opt.MapFrom(src => src.Reference));
            //CreateMap<Request, Q1SubjectPropertyType>()
            //    .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.Property.TitleNumber))
            //    .ForMember(dst => dst.ContinueIfTitleIsClosedAndContinuedIndicator, opt => opt.MapFrom(src => src.Flags.ClosedAndContinued));

            CreateMap<Identity, Q1ExternalReferenceType1>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<string, Q3TextType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q2TextType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType6>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
        }

        public object Item(Request request)
        {
            if (request == null)
                return null;

            var ar = ApplicationReference(request);
            if (ar == null)
                return Property(request);
            return ar;
        }

        public object ApplicationReference(Request request)
        {
            if (request.ApplicationReference == null )
                return null;
            if( string.IsNullOrEmpty(request.ApplicationReference.Reference) )
                return null;

            var ar = request.ApplicationReference.Reference;
            return ar;
        }

        public object Property(Request request)
        {
            if( request == null || request.Property == null || request.Flags == null)
                return null;

            var pt = new Q1SubjectPropertyType();
            pt.TitleNumber = new Q2TextType1() { Value = request.Property.TitleNumber };
            pt.ContinueIfTitleIsClosedAndContinuedIndicator = new IndicatorType() { Value = request.Flags.ClosedAndContinued };

            return pt;
        }
    }
}
