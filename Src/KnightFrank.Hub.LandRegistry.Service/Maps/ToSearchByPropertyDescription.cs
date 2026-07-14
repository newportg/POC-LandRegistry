using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToRequestSearchByPropertyDescriptionMap : Profile
    {
        public ToRequestSearchByPropertyDescriptionMap()
        {
            CreateMap<LandRegistryDto, RequestSearchByPropertyDescriptionV2_0Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType5>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType4>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType4>()
                .ForMember(dst => dst.CustomerReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.SubjectProperty, opt => opt.MapFrom(src => src.Property));

            CreateMap<Identity, Q1CustomerReferenceType2>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<Identity, Q1ExternalReferenceType8>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<Property, Q1SubjectPropertyType2>()
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => src));
            CreateMap<Property, Q1AddressType1>()
                .ForMember(dst => dst.BuildingName, opt => opt.MapFrom(src => src.PropertyName))
                .ForMember(dst => dst.BuildingNumber, opt => opt.MapFrom(src => src.PropertyNumber))
                .ForMember(dst => dst.StreetName, opt => opt.MapFrom(src => src.Line1))
                .ForMember(dst => dst.CityName, opt => opt.MapFrom(src => src.City))
                .ForMember(dst => dst.PostcodeZone, opt => opt.MapFrom(src => src.PostCode));

            CreateMap<string, TextType4>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
        }
    }

}


