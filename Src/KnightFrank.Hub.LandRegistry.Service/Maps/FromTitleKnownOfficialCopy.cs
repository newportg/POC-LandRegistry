using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class FromTitleKnownOfficialCopy : Profile
    {
        public FromTitleKnownOfficialCopy()
        {
            // Level 0
            CreateMap<performTitleKnownSearchResponse, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());

            // Level 1

            // Level 2
            CreateMap<Q1GatewayResponseType5, Response>()
                .ForMember(dst => dst.Acknowledgement, opt => opt.MapFrom(src => src.Acknowledgement.AcknowledgementDetails))
                .ForMember(dst => dst.Rejection, opt => opt.MapFrom(src => src.Rejection))
                .ForMember(dst => dst.Results, opt => opt.MapFrom(src => src.Results))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => StatusType(src.TypeCode.Value)));

            //// Level 3
            //CreateMap<Q1AcknowledgementType5, Acknowledgement>()
            //    .ForMember(dst => dst, opt => opt.MapFrom(src => src.AcknowledgementDetails));

            CreateMap<Q1RejectionType5, Rejection>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.RejectionResponse, opt => opt.MapFrom(src => src.RejectionResponse));

            CreateMap<Q1ResultsType5, Results>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.MessageDetails, opt => opt.MapFrom(src => src));
                //.ForMember(dst => dst.TitleOfficalCopy, opt => opt.MapFrom(src => src));

            CreateMap<Q1ResultsType5, MessageDetails>()
                .ConvertUsing<TitleOfficalCopyMessageDetailConverter>();


            // Level 4
            CreateMap<Q1AcknowledgementDetailsType5, Acknowledgement>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueID.Value))
                .ForMember(dst => dst.ExpectedResponseDateTime, opt => opt.MapFrom(src => src.ExpectedResponseDateTime.Value))
                .ForMember(dst => dst.MessageDescription, opt => opt.MapFrom(src => src.MessageDescription.Value))
                .ForMember(dst => dst.HMLRReference, opt => opt.Ignore());

            CreateMap<Q1RejectionResponseType5, RejectionResponse>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Reason, opt => opt.MapFrom(src => src.Reason.Value))
                .ForMember(dst => dst.Errors, opt => opt.MapFrom(src => src.ValidationErrors));

            CreateMap<Q1ValidationErrorsType5, Error>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description.Value));

            // Simple Types
            CreateMap<ProductResponseCodeType5, string>()
                .ConvertUsing((src, dst, context) =>
                {
                    dst = StatusType(src.Value);
                    return dst;
                });
        }

        private string StatusType(ProductResponseCodeContentType5 code)
        {
            switch (code)
            {
                case ProductResponseCodeContentType5.Item0:
                    return "";
                case ProductResponseCodeContentType5.Item10:
                    return "Acknowledgement";
                case ProductResponseCodeContentType5.Item20:
                    return "Rejection";
                case ProductResponseCodeContentType5.Item30:
                    return "Success";
            }
            return "Invalid";
        }
    }

    public class TitleOfficalCopyMessageDetailConverter : ITypeConverter<Q1ResultsType5, MessageDetails>
    {
        public MessageDetails Convert(Q1ResultsType5 source, MessageDetails destination, ResolutionContext context)
        {
            var md = new TitleOfficalCopyMessageDetails()
            {
                HMLRReference = (source.HMLRReference.Reference.Value != null) ? source.HMLRReference.Reference.Value : null,
                ResultTypeCode = ResultTypeCodeContentType(source.ResultTypeCode),
                ActualPrice = source.ActualPrice.GrossPriceAmount.Value,
                Attachment = new Attachment()
                {
                    Title = (source.Attachment.Title != null) ? source.Attachment.Title.Value : default,
                    Description = (source.Attachment.Description != null) ? source.Attachment.Description.Value : default,
                    Date = (source.Attachment.Date != null) ? source.Attachment.Date.Value : default,
                    CopyrightNotices = (source.Attachment.CopyrightNotices != null) ? source.Attachment.CopyrightNotices.Value : default,
                    EmbeddedFileBinaryObject = new KnightFrank.Hub.LandRegistry.Common.Models.BinaryObjectType()
                    {
                        filename = (source.Attachment.EmbeddedFileBinaryObject.filename != null) ? source.Attachment.EmbeddedFileBinaryObject.filename : default,
                        format = (source.Attachment.EmbeddedFileBinaryObject.format != null) ? source.Attachment.EmbeddedFileBinaryObject.format : default,
                        mimeCode = (source.Attachment.EmbeddedFileBinaryObject.mimeCode != null) ? source.Attachment.EmbeddedFileBinaryObject.mimeCode : default,
                        characterSetCode = (source.Attachment.EmbeddedFileBinaryObject.characterSetCode != null) ? source.Attachment.EmbeddedFileBinaryObject.characterSetCode : default,
                        Value = (source.Attachment.EmbeddedFileBinaryObject.Value != null) ? source.Attachment.EmbeddedFileBinaryObject.Value : default
                    }
                },
                Description = (source.MessageDetails != null) ? source.MessageDetails.Description.Value : default
            };

            return md;
        }

        private string ResultTypeCodeContentType(ResultCodeType2 tcode)
        {
            if (tcode == null)
                return "Unknown";

            var code = tcode.Value.ToString();

            if (code.ToLower().Equals("item0")) return "Other";
            if (code.ToLower().Equals("item10")) return "10";
            if (code.ToLower().Equals("item20")) return "20";
            if (code.ToLower().Equals("item30")) return "30";
            if (code.ToLower().Equals("item40")) return "40";
            if (code.ToLower().Equals("item40")) return "50";

            return code;
        }
    }

}
