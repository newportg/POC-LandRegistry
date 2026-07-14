using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToTitleKnownOfficialCopy : Profile
    {
        public ToTitleKnownOfficialCopy()
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestTitleKnownOfficialCopyV2_1Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType8>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType7>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType7>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.CustomerReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.ExpectedPrice, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.Contact, opt => opt.MapFrom(src => new[] { src.Contact }))
                .ForMember(dst => dst.DocumentDetails, opt => opt.MapFrom(src => src.DocumentInfo))
                .ForMember(dst => dst.SubjectProperty, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.TitleKnownOfficialCopy, opt => opt.MapFrom(src => src))
                .ForMember(dst => dst.AlternativeDespatchDetails, opt => opt.MapFrom(src => src.AlternativeDespatchDetails));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchDetailsType4>()
                .ForMember(dst => dst.AlternativeDespatchReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.AlternativeDespatchName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.AlternativeDespatchAddress, opt => opt.MapFrom(src => src));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchAddressType2>()
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => src));
            CreateMap<AlternativeDespatchDetails, Q1AlternativePostalAddressType2>()
                .ForMember(dst => dst.AddressLine, opt => opt.MapFrom( src => src.Address))
                .ForMember(dst => dst.Postcode, opt => opt.MapFrom(src => src.PostCode));

            CreateMap<Identity, Q1ExternalReferenceType12>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Identity, Q1CustomerReferenceType5>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Contact, Q1ContactType4>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Communication, opt => opt.MapFrom(src => src));
            CreateMap<Contact, Q1CommunicationType4>()
                .ForMember(dst => dst.Telephone, opt => opt.MapFrom(src => src.Telephone));

            CreateMap<Property, Q1SubjectPropertyType3>()
                .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.TitleNumber))
                .ForMember(dst => dst.TenureTypeCode, opt => opt.MapFrom(src => src.Tenure));

            CreateMap<Property, Q1ExpectedPriceType4>()
                .ForMember(dst => dst.GrossPriceAmount, opt => opt.MapFrom(src => src.ExpectedPrice))
                .ForMember(dst => dst.NetPriceAmount, opt => opt.MapFrom(src => src.NetPriceAmount))
                .ForMember(dst => dst.VATAmount, opt => opt.MapFrom(src => src.VATAmount));

            CreateMap<decimal, AmountType6>()
                .ForMember(dst => dst.Value, opt => {
                    opt.Condition(src => src != decimal.Zero);
                    opt.MapFrom(src => src);              
                })
                .ForMember(dst => dst.currencyID, opt => opt.MapFrom(src => ""));

            CreateMap<DocumentInfo, Q1DocumentDetailsType>()
                .ForMember(dst => dst.DateOfDocumentDate, opt => opt.MapFrom(src => src.DocumentDate))
                .ForMember(dst => dst.TypeOfDocumentCode, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dst => dst.AdditionalInformation, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dst => dst.TitleNumberFiledUnder, opt => opt.MapFrom(src => src.TitleNumberFiledUnder));

            CreateMap<Request, Q1TitleKnownOfficialCopyType>()
                .ForMember(dst => dst.RequestedOfficialCopyCode, opt => opt.MapFrom(src => src.Flags))
                .ForMember(dst => dst.OfficialCopyTypeCode, opt => opt.MapFrom(src => src.Flags))
                .ForMember(dst => dst.ContinueIfActualFeeExceedsExpectedFeeIndicator, opt => opt.MapFrom(src => src.Flags.ContinueIfFeeExceeds))
                .ForMember(dst => dst.ContinueIfTitleIsClosedAndContinuedIndicator, opt => opt.MapFrom(src => src.Flags.ClosedAndContinued))
                .ForMember(dst => dst.NotifyIfPendingApplicationIndicator, opt => opt.MapFrom(src => src.Flags.PendingApps))
                .ForMember(dst => dst.SendBackDatedIndicator, opt => opt.MapFrom(src => src.Flags.SendBackdated))
                .ForMember(dst => dst.NotifyIfPendingFirstRegistrationIndicator, opt => opt.MapFrom(src => src.Flags.FirstRegistration))
                .ForMember(dst => dst.PropertyDescription, opt => opt.MapFrom(src => src.Property.PropertyDescription ?? string.Empty))
                .ForMember(dst => dst.CertificateInFormCI, opt =>
                {
                    opt.PreCondition(src => src.Flags != null && src.Flags.CertificateInFormCI != null); // If this is != null
                    opt.MapFrom(src => src.Flags.CertificateInFormCI);              // Then do this --- Yea
                });

            CreateMap<Flags, RequestedOfficialCopyCodeType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => TypeOfRequestedOfficialCopyCodeContentType(src.RequestedOfficialCopy)));
            CreateMap<Flags, OfficialCopyCodeType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => TypeOfOfficialCopyCodeType(src.OfficialCopy)));

            // Simple Maps
            CreateMap<string, TenureCodeType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<DateTime, DateType5>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, TypeOfDocumentCodeType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => TypeOfDocumentCodeContent(src)));
            CreateMap<string, Q2TextType4>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType2>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType4>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType6>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType6>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<decimal, NumericType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            //CreateMap<decimal, AmountType1>()
            //    .ForMember(dst => dst.Value, opt =>
            //    {
            //        opt.PreCondition( src => src != 0);
            //        opt.MapFrom( src => src);
            //    });
        }

        private static OfficialCopyCodeContentType TypeOfOfficialCopyCodeType(string code)
        {
            return code switch
            {
                "OC2" => OfficialCopyCodeContentType.Item20,
                _ => OfficialCopyCodeContentType.Item10
            };
        }
        private static RequestedOfficialCopyCodeContentType TypeOfRequestedOfficialCopyCodeContentType(string code)
        {
            return code switch
            {
                "RegisterOnly" => RequestedOfficialCopyCodeContentType.Item10,
                "TitleOnly" => RequestedOfficialCopyCodeContentType.Item20,
                "RegisterAndTitlePlan" => RequestedOfficialCopyCodeContentType.Item30,
                "CI" => RequestedOfficialCopyCodeContentType.Item40,
                "CIAndRegister" => RequestedOfficialCopyCodeContentType.Item50,
                _ => RequestedOfficialCopyCodeContentType.Item10
            };
        }
        private static TypeOfDocumentCodeContentType TypeOfDocumentCodeContent(string code)
        {
            return code switch
            {
                "Abstract" => TypeOfDocumentCodeContentType.Item10,
                "Agreement" => TypeOfDocumentCodeContentType.Item20,
                "Assent" => TypeOfDocumentCodeContentType.Item30,
                "Assignment" => TypeOfDocumentCodeContentType.Item40,
                "Charge" => TypeOfDocumentCodeContentType.Item50,
                "Conveyance" => TypeOfDocumentCodeContentType.Item60,
                "Deed" => TypeOfDocumentCodeContentType.Item70,
                "Indenture" => TypeOfDocumentCodeContentType.Item80,
                "Lease" => TypeOfDocumentCodeContentType.Item90,
                "Licence" => TypeOfDocumentCodeContentType.Item100,
                "Plan" => TypeOfDocumentCodeContentType.Item110,
                "Sub - Charge" => TypeOfDocumentCodeContentType.Item120,
                "Transfer" => TypeOfDocumentCodeContentType.Item130,
                "Other" => TypeOfDocumentCodeContentType.Item140,
                "Commonhold Community Statement" => TypeOfDocumentCodeContentType.Item150,
                "Memorandum and Articles of Association" => TypeOfDocumentCodeContentType.Item160,
                "Surrender of Development Rights" => TypeOfDocumentCodeContentType.Item170,
                "Termination Document" => TypeOfDocumentCodeContentType.Item180,
                _ => TypeOfDocumentCodeContentType.Item10
            };
        }
    }
}
