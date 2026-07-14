using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class FromDischargeActivity : Profile
    {
        public FromDischargeActivity() 
        {
            // Level 0
            CreateMap<dischargeActivityResponse, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());


            // Poll Response
            CreateMap<getResponseResponse3, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());

            // Level 1

            // Level 2
            CreateMap<Q1GatewayResponseType2, Response>()
                .ForMember(dst => dst.Acknowledgement, opt => opt.MapFrom(src => src.Acknowledgement.AcknowledgementDetails))
                .ForMember(dst => dst.Rejection, opt => opt.MapFrom(src => src.Rejection))
                .ForMember(dst => dst.Results, opt => opt.MapFrom(src => src.Results))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => StatusType(src.TypeCode.Value)));

            // Level 3
            //CreateMap<Q1AcknowledgementType2, Acknowledgement>()
            //    .ForMember(dst => dst, opt => opt.MapFrom(src => src.AcknowledgementDetails));

            CreateMap<Q1RejectionType2, Rejection>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.RejectionResponse, opt => opt.MapFrom(src => src.RejectionResponse));

            CreateMap<Q1ResultsType2, Results>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.MessageDetails, opt => opt.MapFrom(src => src));

            CreateMap<Q1ResultsType2, MessageDetails>()
                .ConvertUsing<DischargeActivityMessageDetailConverter>();


            // Level 4
            CreateMap<Q1AcknowledgementDetailsType2, Acknowledgement>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueID.Value))
                .ForMember(dst => dst.ExpectedResponseDateTime, opt => opt.MapFrom(src => src.ExpectedResponseDateTime.Value))
                .ForMember(dst => dst.MessageDescription, opt => opt.MapFrom(src => src.MessageDescription.Value))
                .ForMember(dst => dst.HMLRReference, opt => opt.Ignore());

            CreateMap<Q1RejectionResponseType2, RejectionResponse>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Reason, opt => opt.MapFrom(src => src.Reason.Value))
                .ForMember(dst => dst.Errors, opt => opt.MapFrom(src => src.ValidationErrors));

            CreateMap<Q1ValidationErrorsType, Error>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description.Value));
        }

        private string StatusType(ProductResponseCodeContentType2 code)
        {
            switch (code)
            {
                case ProductResponseCodeContentType2.Item0:
                    return "";
                case ProductResponseCodeContentType2.Item10:
                    return "Acknowledgement";
                case ProductResponseCodeContentType2.Item20:
                    return "Rejection";
                case ProductResponseCodeContentType2.Item30:
                    return "Success";
            }
            return "Invalid";
        }
    }

    public class DischargeActivityMessageDetailConverter : ITypeConverter<Q1ResultsType2, MessageDetails>
    {
        public MessageDetails Convert(Q1ResultsType2 source, MessageDetails destination, ResolutionContext context)
        {
            var md = new DischargeActivityMessageDetails()
            //var md = new MessageMessageDetails()
            {

                ResultDateTime = (source.ResultDateTime != null) ? source.ResultDateTime.Value : default,
                TitleNumber = (source.Title != null) ? source.Title.TitleNumber.Value : default,
                Description = (source.MessageDetails != null) ? source.MessageDetails.Description.Value : default,
                DischargeCount = (source.DischargeCount != null) ? source.DischargeCount.Value : 0
            };

            if (source.DischargeCount != null && source.DischargeCount.Value > 0)
            {
                md.Discharges = new Discharges[source.DischargeCount.Value - 1];

                for (var i = 0; i < source.DischargeCount.Value - 1; i++)
                {
                    var dis = new Discharges()
                    {
                        DischargeDate = (source.Discharges[i].DischargeDate != null) ? source.Discharges[i].DischargeDate.Value : default,
                        ChargeProprietors = new string[source.Discharges[i].ChargeProprietors.Length]
                    };

                    for (var j = 0; j < source.Discharges[i].ChargeProprietors.Length; j++)
                    {
                        dis.ChargeProprietors[j] = source.Discharges[i].ChargeProprietors[j].Value;
                    }

                    md.Discharges[i] = dis;
                }
            }


            return md;
        }
    }
}
