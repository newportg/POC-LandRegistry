using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class FromApplicationEnquiry : Profile
    {
        public FromApplicationEnquiry()
        {
            // Level 0
            CreateMap<applicationEnquiryResponse, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());

            // Poll Response
            CreateMap<getResponseResponse, LandRegistryDto>()
                .ForMember(dst => dst.Response, opt => opt.MapFrom(src => src.@return.GatewayResponse))
                .ForMember(dst => dst.RequestType, opt => opt.Ignore())
                .ForMember(dst => dst.Request, opt => opt.Ignore())
                .ForMember(dst => dst.SystemError, opt => opt.Ignore());

            // Level 1

            // Level 2
            CreateMap<Q1GatewayResponseType, Response>()
                .ForMember(dst => dst.Acknowledgement, opt => opt.MapFrom(src => src.Acknowledgement.AcknowledgementDetails))
                .ForMember(dst => dst.Rejection, opt => opt.MapFrom(src => src.Rejection))
                .ForMember(dst => dst.Results, opt => opt.MapFrom(src => src.Results))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => StatusType(src.TypeCode.Value)));

            // Level 3
            //CreateMap<Q1AcknowledgementType, Acknowledgement>()
            //    .ForMember(dst => dst, opt => opt.MapFrom(src => src.AcknowledgementDetails));

            CreateMap<Q1RejectionType, Rejection>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.RejectionResponse, opt => opt.MapFrom(src => src.RejectionResponse));

            CreateMap<Q1ResultsType, Results>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.ExternalReference.Reference.Value))
                .ForMember(dst => dst.MessageDetails, opt => opt.MapFrom(src => src));

            CreateMap<Q1ResultsType, MessageDetails>()
                .ConvertUsing<ApplicationEnquiryMessageDetailConverter>();

            // Level 4
            CreateMap<Q1AcknowledgementDetailsType, Acknowledgement>()
                .ForMember(dst => dst.UniqueMsgId, opt => opt.MapFrom(src => src.UniqueID.Value))
                .ForMember(dst => dst.ExpectedResponseDateTime, opt => opt.MapFrom(src => src.ExpectedResponseDateTime.Value))
                .ForMember(dst => dst.MessageDescription, opt => opt.MapFrom(src => src.MessageDescription.Value))
                .ForMember(dst => dst.HMLRReference, opt => opt.Ignore());

            CreateMap<Q1RejectionResponseType, RejectionResponse>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Reason, opt => opt.MapFrom(src => src.Reason.Value))
                .ForMember(dst => dst.Errors, opt => opt.MapFrom(src => src.ValidationErrors));

            CreateMap<Q1ValidationErrorsType, Error>()
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code.Value))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description.Value));
        }

        private static string StatusType(ProductResponseCodeContentType code)
        {
            switch (code)
            {
                case ProductResponseCodeContentType.Item0:
                    return "";
                case ProductResponseCodeContentType.Item10:
                    return "Acknowledgement";
                case ProductResponseCodeContentType.Item20:
                    return "Rejection";
                case ProductResponseCodeContentType.Item30:
                    return "Success";
                default:
                    break;
            }
            return "Invalid";
        }
    }

    public class ApplicationEnquiryMessageDetailConverter : ITypeConverter<Q1ResultsType, MessageDetails>
    {
        public MessageDetails Convert(Q1ResultsType source, MessageDetails destination, ResolutionContext context)
        {
            var md = new ApplicationEnquiryMessageDetails()
            //var md = new MessageMessageDetails()
            {
                Title = (source.Title != null) ? new Title() { TitleNumber = source.Title.TitleNumber.Value } : null,
                DateTime = (source.ResultDateTime != null) ? source.ResultDateTime.Value : default,
                Description = (source.MessageDetails != null) ? source.MessageDetails.Description.Value : default,
                RecentApplication = (source.MessageDetails.RecentApplication != null)
                    ?
                        new RecentApplication()
                        {
                            EndReason = source.MessageDetails.RecentApplication.EndReason.ToString(),
                            EndDate = source.MessageDetails.RecentApplication.EndDate.Value
                        }
                    : default
            };
            if (source.Title != null)
            {
                md.Titles = new Title[1];
                md.Titles[0] = new Title { TitleNumber = source.Title.TitleNumber.Value };
            }

            if (source.ApplicationEnquiry != null && source.ApplicationEnquiry.Length > 0)
            {
                md.ApplicationEnquiry = new ApplicationEnquiry[source.ApplicationEnquiry.Length];
                for (var i = 0; i < source.ApplicationEnquiry.Length; i++)
                {
                    var ae = new ApplicationEnquiry
                    {
                        ApplicationType = source.ApplicationEnquiry[i].ApplicationType.ToString(),
                        Applicant = (source.ApplicationEnquiry[i].Applicant != null) ? source.ApplicationEnquiry[i].Applicant.Value : default,
                        ApplicationReference = (source.ApplicationEnquiry[i].ApplicationReference != null) ? source.ApplicationEnquiry[i].ApplicationReference : default,
                        CustomerReference = (source.ApplicationEnquiry[i].CustomerReference != null) ? source.ApplicationEnquiry[i].CustomerReference : default,
                        ApplicationReceivedBy =source.ApplicationEnquiry[i].ApplicationReceivedBy.GetType().Name,
                        PropertyDescription = (source.ApplicationEnquiry[i].PropertyDescription != null) ? source.ApplicationEnquiry[i].PropertyDescription : default,
                        LodgedBy = (source.ApplicationEnquiry[i].LodgedBy != null) ? source.ApplicationEnquiry[i].LodgedBy : default,
                    };

                    if (source.ApplicationEnquiry[i].ApplicationProgress != null)
                    {
                        ae.ApplicationProgress = new ApplicationProgress
                        {
                            Description = (source.ApplicationEnquiry[i].ApplicationProgress.Description != null) ? source.ApplicationEnquiry[i].ApplicationProgress.Description.Value : default
                        };

                        if (source.ApplicationEnquiry[i].ApplicationProgress.Correspondence != null)
                        {
                            ae.ApplicationProgress.Correspondence = new Correspondence[source.ApplicationEnquiry[i].ApplicationProgress.Correspondence.Length];
                            for (var j = 0; j < source.ApplicationEnquiry[i].ApplicationProgress.Correspondence.Length; j++)
                            {
                                var cor = new Correspondence
                                {
                                    CorrespondenceType = source.ApplicationEnquiry[i].ApplicationProgress.Correspondence[j].CorrespondenceType.ToString(),
                                    ExpiresOnDate = (source.ApplicationEnquiry[i].ApplicationProgress.Correspondence[j].ExpiresOnDate != null) ? source.ApplicationEnquiry[i].ApplicationProgress.Correspondence[j].ExpiresOnDate.Value : default
                                };

                                if (source.ApplicationEnquiry[i].ApplicationProgress.Correspondence[j].ItemElementName.ToString() == "RequestedOnDate")
                                    cor.RequestedOnDate = source.ApplicationEnquiry[i].ApplicationProgress.Correspondence[j].Item.Value;
                                else
                                    cor.IssuedOnDate = source.ApplicationEnquiry[i].ApplicationProgress.Correspondence[j].Item.Value;

                                ae.ApplicationProgress.Correspondence[j] = cor;
                            }
                        }
                    }

                    md.ApplicationEnquiry[i] = ae;
                }

            }

            return md;
        }

        //private string ApplicationType(ApplicationTypeContentType tcode)
        //{
        //    var code = tcode.ToString();

        //    if (code.ToLower().Equals("item0")) return "Other";
        //    if (code.ToLower().Equals("item10")) return "10";
        //    if (code.ToLower().Equals("item20")) return "20";
        //    if (code.ToLower().Equals("item30")) return "30";
        //    if (code.ToLower().Equals("item40")) return "40";
        //    if (code.ToLower().Equals("item40")) return "50";
        //    if (code.ToLower().Equals("item40")) return "60";
        //    if (code.ToLower().Equals("item40")) return "70";
        //    if (code.ToLower().Equals("item40")) return "80";
        //    if (code.ToLower().Equals("item40")) return "90";

        //    return code;
        //}
    }

}
