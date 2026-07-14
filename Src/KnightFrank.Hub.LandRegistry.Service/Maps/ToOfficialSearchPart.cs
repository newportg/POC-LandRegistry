using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToOfficialSearchPart : Profile
    {
        public ToOfficialSearchPart()
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestOfficialSearchOfPartWithPriorityV2_1Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType11>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType10>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType10>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.CustomerReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.ExpectedPrice, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.Contact, opt => opt.MapFrom(src => Contact(src.Contact)))
                .ForMember(dst => dst.SubjectProperty, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.OfficialSearchOfPartWithPriority, opt => opt.MapFrom(src => src.PrioritySearch))
                .ForMember(dst => dst.AlternativeDespatchDetails, opt => opt.MapFrom(src => src.AlternativeDespatchDetails));

            CreateMap<Identity, Q1ExternalReferenceType18>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Identity, Q1CustomerReferenceType8>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<Property, Q1ExpectedPriceType7>()
                .ForMember(dst => dst.GrossPriceAmount, opt => opt.MapFrom(src => src.ExpectedPrice))
                .ForMember(dst => dst.NetPriceAmount, opt => opt.MapFrom(src => src.NetPriceAmount))
                .ForMember(dst => dst.VATAmount, opt => opt.MapFrom(src => src.VATAmount));

            CreateMap<Property, Q1SubjectPropertyType6>()
                .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.TitleNumber));

            CreateMap<PrioritySearch, Q1OfficialSearchOfPartWithPriorityType1>()
                .ForMember(dst => dst.PriorityTypeCode, opt => opt.MapFrom(src => src.PriorityType))
                .ForMember(dst => dst.SearchFromDate, opt => opt.MapFrom(src => src.SearchFrom))
                .ForMember(dst => dst.ApplicantParty, opt => opt.MapFrom(src => src.ApplicantNames))
                .ForMember(dst => dst.PropertyIdentification, opt => opt.MapFrom(src => src.PropertyIdentification))
                .ForMember(dst => dst.TimeshareDetails, opt => opt.MapFrom(src => src.TimeshareDetails))
                .ForMember(dst => dst.RegisteredProprietorParty, opt => opt.MapFrom(src => src.PropreietorOrFirstApplicant))
                .ForMember(dst => dst.ContinueIfActualFeeExceedsExpectedFeeIndicator, opt => opt.MapFrom(src => src.ContinueIfFeeExceeds))
                .ForMember(dst => dst.ContinueIfNameMismatchOnRegisterIndicator, opt => opt.MapFrom(src => src.ContinueIfNameMismatch));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchDetailsType6>()
                .ForMember(dst => dst.AlternativeDespatchReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.AlternativeDespatchName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.AlternativeDespatchAddress, opt => opt.MapFrom(src => src));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchAddressType3>()
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => src));
            CreateMap<AlternativeDespatchDetails, Q1AlternativePostalAddressType3>()
                .ForMember(dst => dst.AddressLine, opt => opt.MapFrom(src => src.Address))
                .ForMember(dst => dst.Postcode, opt => opt.MapFrom(src => src.PostCode));
            CreateMap<AlternativeDespatchDetails, Q1DXDetailsType3>()
                .ForMember(dst => dst.DXNumber, opt => opt.MapFrom(src => src.DXNumber))
                .ForMember(dst => dst.ExchangeName, opt => opt.MapFrom(src => src.ExchangeName));

            CreateMap<decimal, AmountType12>()
                .ForMember(dst => dst.Value, opt => {
                    opt.Condition(src => src != decimal.Zero);
                    opt.MapFrom(src => src);
                })
                .ForMember(dst => dst.currencyID, opt => opt.MapFrom(src => ""));

            CreateMap<PropertyIdentification, Q1PropertyIdentificationType1>()
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => PropertyId(src)));

            //CreateMap<PlanAttachment, Q1PlanAttachmentType1>()
            //    .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date))
            //    .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dst => dst.CopyrightNotices, opt => opt.MapFrom(src => src.CopyrightNotices))
            //    .ForMember(dst => dst.EmbeddedFileBinaryObject, opt => opt.MapFrom(src => src.BinaryObject));

            //CreateMap<string, Q1TimeshareDetailsType1>()
            //    .ForMember(dst => dst., opt => opt.MapFrom(src => src));

            CreateMap<TimeshareDetails, Q1TimeshareDetailsType1>()
                .ForMember(dst => dst.SearchInRespectOfDiscontinuousTimeshareLeaseIndicator, opt => opt.MapFrom(src => src.TimeshareLease))
                .ForMember(dst => dst.TimeshareTimePeriodDemised, opt => opt.MapFrom(src => src.TimePeriod));


            CreateMap<string, Q2TextType8>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType5>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType9>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType10>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, PriorityCodeType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => PriorityCodeType(src)));
            CreateMap<DateTime, SearchFromDateType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.ToString("yyyy-MM-dd")));
            CreateMap<string, Q1ApplicantPartyType1>()
                .ForMember(dst => dst.ApplicantName, opt => opt.MapFrom(src => src));
            CreateMap<string, Q1RegisteredProprietorPartyType1>()
                .ForMember(dst => dst.PropreitorName, opt => opt.MapFrom(src => src));

            CreateMap<string, Q1TimeshareTimePeriodDemisedType1>()
                .ForMember(dst => dst.TimesharePeriod, opt => opt.MapFrom(src => src));
            CreateMap<string, Q1TimesharePeriodTextType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));

            CreateMap<DateTime, DateType10>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.ToString("yyyy-MM-dd")));

            CreateMap<Common.Models.BinaryObjectType, BinaryObjectType6>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src))
                .ForMember(dst => dst.filename, opt => opt.MapFrom(src => src))
                .ForMember(dst => dst.characterSetCode, opt => opt.MapFrom(src => src))
                .ForMember(dst => dst.mimeCode, opt => opt.MapFrom(src => src));
        }

        public Q1ContactType6[] Contact(Contact cont)
        {
            if (cont == null)
                return null;

            var contact = new Q1ContactType6[1];

            var con = new Q1ContactType6()
            {
                Name = cont.Name, //   new Q3TextType9() { Value = cont.Name },
                Communication = new Q1CommunicationType6() { Telephone = new Q3TextType9() { Value = cont.Telephone } }
            };

            contact[0] = con;
            return contact;
        }

        public PriorityCodeContentType1 PriorityCodeType(string type)
        {
            if (type.Equals("Purchase")) return PriorityCodeContentType1.Item10;
            if (type.Equals("Lease")) return PriorityCodeContentType1.Item20;
            if (type.Equals("Charge")) return PriorityCodeContentType1.Item30;
            return PriorityCodeContentType1.Item10;
        }

        public object PropertyId(PropertyIdentification src)
        {
            if (src == null) return null;

            if (!string.IsNullOrEmpty(src.PropertyDescription))
                return src.PropertyDescription;
            else if (src.TitlePlan != null)
            {
                var rtn = new Q1TitlePlanType1();
                rtn.TitlePlanNumber = new Q2TextType8() { Value = src.TitlePlan.TitlePlanNumber };
                rtn.Reference = new Q3TextType9() { Value = src.TitlePlan.Reference };

                return rtn;
            }
            else if (src.EstatePlan != null)
            {
                var rtn = new Q1EstatePlanType1();
                rtn.ApprovalDate = new DateType10() { Value = src.EstatePlan.ApprovalDate };
                rtn.PlotDetails = src.EstatePlan.PlotDetails;
                return rtn;
            }
           
            return null;
        }
    }
}
