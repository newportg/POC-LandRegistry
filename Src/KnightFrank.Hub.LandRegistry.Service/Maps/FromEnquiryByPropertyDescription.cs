using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class FromEnquiryByPropertyDescription : Profile
    {
        public FromEnquiryByPropertyDescription()
        {
            // Level 0
            CreateMap<searchPropertiesResponse, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());

            // Poll Response
            CreateMap<getResponseResponse4, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());

            // Level 1

            // Level 2
            CreateMap<Q1GatewayResponseType3, Response>()
                .ForMember(dst => dst.Acknowledgement, opt => opt.MapFrom(src => src.Acknowledgement.AcknowledgementDetails))
                .ForMember(dst => dst.Rejection, opt => opt.MapFrom(src => src.Rejection))
                .ForMember(dst => dst.Results, opt => opt.MapFrom(src => src.Results))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => StatusType(src.TypeCode.Value)));

            // Level 3
            //CreateMap<Q1AcknowledgementType3, Acknowledgement>()
            //    .ForMember(dst => dst, opt => opt.MapFrom(src => src.AcknowledgementDetails));

            CreateMap<Q1RejectionType3, Rejection>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.RejectionResponse, opt => opt.MapFrom(src => src.RejectionResponse));

            CreateMap<Q1ResultsType3, Results>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.MessageDetails, opt => opt.MapFrom(src => src));

            // Level 4
            CreateMap<Q1AcknowledgementDetailsType3, Acknowledgement>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueID.Value))
                .ForMember(dst => dst.ExpectedResponseDateTime, opt => opt.MapFrom(src => src.ExpectedResponseDateTime.Value))
                .ForMember(dst => dst.MessageDescription, opt => opt.MapFrom(src => src.MessageDescription.Value))
                .ForMember(dst => dst.HMLRReference, opt => opt.Ignore());

            CreateMap<Q1RejectionResponseType3, RejectionResponse>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Reason, opt => opt.MapFrom(src => src.Reason.Value))
                .ForMember(dst => dst.Errors, opt => opt.MapFrom(src => src.ValidationErrors));

            CreateMap<Q1ValidationErrorsType3, Error>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description.Value));

            CreateMap<Q1ResultsType3, MessageDetails>()
                 .ConvertUsing<MessageDetailConverter>();
        }

        private string StatusType(ProductResponseCodeContentType3 code)
        {
            switch (code)
            {
                case ProductResponseCodeContentType3.Item0:
                    return "";
                case ProductResponseCodeContentType3.Item10:
                    return "Acknowledgement";
                case ProductResponseCodeContentType3.Item20:
                    return "Rejection";
                case ProductResponseCodeContentType3.Item30:
                    return "Success";
            }
            return "Invalid";
        }
    }

    public class MessageDetailConverter : ITypeConverter<Q1ResultsType3, MessageDetails>
    {
        public MessageDetails Convert(Q1ResultsType3 source, MessageDetails destination, ResolutionContext context)
        {
            var md = new EnquiryByPropertyDescriptionMessageDetails();

            if (source.MessageDetails != null)
                md.Description = (source.MessageDetails.Description != null) ? source.MessageDetails.Description.Value : default;
            md.Titles = new Title[source.Title.Length];

            var i = 0;
            foreach (var item in source.Title)
            {
                Title title = new Title
                {
                    TitleNumber = (item.TitleNumber != null) ? item.TitleNumber.Value : default,
                    Description = (item.Description != null) ? item.Description.Value : default,
                    TenureTypeCode = TenureCodeContentType(item.TenureInformation.TenureTypeCode),
                    Address = new Address()
                    {
                        BuildingName = (item.Address.BuildingName != null) ? item.Address.BuildingName.Value : default,
                        SubBuildingName = (item.Address.SubBuildingName != null) ? item.Address.SubBuildingName.Value : default,
                        BuildingNumber = (item.Address.BuildingNumber != null) ? item.Address.BuildingNumber.Value : default,
                        StreetName = (item.Address.StreetName != null) ? item.Address.StreetName.Value : default,
                        CityName = (item.Address.CityName != null) ? item.Address.CityName.Value : default,
                        PostcodeZone = (item.Address.PostcodeZone != null) ? item.Address.PostcodeZone.Postcode.Value : default
                    }
                };

                md.Titles[i++] = title;
            }

            return md;
        }

        private string TenureCodeContentType(TenureCodeType tcode)
        {
            if (tcode == null)
                return "Unknown";

            var code = tcode.Value.ToString();

            if (code.ToLower().Equals("item0")) return "Other";
            if (code.ToLower().Equals("item10")) return "Freehold";
            if (code.ToLower().Equals("item20")) return "Leasehold";
            if (code.ToLower().Equals("item30")) return "Commonhold";
            if (code.ToLower().Equals("item40")) return "Feuhold";

            if (code.ToLower().Equals("item100")) return "Mixed";
            if (code.ToLower().Equals("item110")) return "Unknown";
            if (code.ToLower().Equals("item120")) return "Unavailable";
            if (code.ToLower().Equals("item130")) return "Caution Against First Registratio";
            if (code.ToLower().Equals("item140")) return "Rent Charge";
            if (code.ToLower().Equals("item150")) return "Franchise";
            if (code.ToLower().Equals("item160")) return "Profit A Prendre In Gross";
            if (code.ToLower().Equals("item170")) return "Manor";

            return code;
        }
    }
}
