using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToOfficialSearchWhole : Profile
    {
        public ToOfficialSearchWhole() 
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestOfficialSearchOfWholeWithPriorityV2_1Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType13>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType12>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType12>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.CustomerReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.ExpectedPrice, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.Contact, opt => opt.MapFrom(src => Contact(src.Contact)))
                .ForMember(dst => dst.SubjectProperty, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.OfficialSearchOfWholeWithPriority, opt => opt.MapFrom(src => src.PrioritySearch))
                .ForMember(dst => dst.AlternativeDespatchDetails, opt => opt.MapFrom(src => src.AlternativeDespatchDetails));

            CreateMap<Identity, Q1ExternalReferenceType21>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Identity, Q1CustomerReferenceType10>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<PrioritySearch, Q1OfficialSearchOfWholeWithPriorityType1>()
                .ForMember(dst => dst.PriorityTypeCode, opt => opt.MapFrom(src => src.PriorityType))
                .ForMember(dst => dst.PropertyDescription, opt => opt.MapFrom(src => src.PropertyDescription))
                .ForMember(dst => dst.SearchFromDate, opt => opt.MapFrom(src => src.SearchFrom))
                .ForMember(dst => dst.ApplicantParty, opt => opt.MapFrom(src => src.ApplicantNames))
                .ForMember(dst => dst.RegisteredProprietorsDetailsOrApplicantsForFirstRegistration, opt => opt.MapFrom(src => src.PropreietorOrFirstApplicant))

                .ForMember(dst => dst.ContinueIfActualFeeExceedsExpectedFeeIndicator, opt => opt.MapFrom(src => src.ContinueIfFeeExceeds))
                .ForMember(dst => dst.ContinueIfNameMismatchOnRegisterIndicator, opt => opt.MapFrom(src => src.ContinueIfNameMismatch))
                .ForMember(dst => dst.ContinueIfDeveloperTitleIndicator, opt => opt.MapFrom(src => src.ContinueIfDeveloperTitle))
                .ForMember(dst => dst.ContinueIfPendingSearchesOfPartIndicator, opt => opt.MapFrom(src => src.ContinueIfPendingSearchesOfPart));

            CreateMap<Property, Q1SubjectPropertyType8>()
                .ForMember(dst => dst.TitleNumber, opt => opt.MapFrom(src => src.TitleNumber));


            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchDetailsType8>()
                .ForMember(dst => dst.AlternativeDespatchReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.AlternativeDespatchName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.AlternativeDespatchAddress, opt => opt.MapFrom(src => src));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchAddressType4>()
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => src));
            CreateMap<AlternativeDespatchDetails, Q1AlternativePostalAddressType3>()
                .ForMember(dst => dst.AddressLine, opt => opt.MapFrom(src => src.Address))
                .ForMember(dst => dst.Postcode, opt => opt.MapFrom(src => src.PostCode));
            CreateMap<AlternativeDespatchDetails, Q1DXDetailsType3>()
                .ForMember(dst => dst.DXNumber, opt => opt.MapFrom(src => src.DXNumber))
                .ForMember(dst => dst.ExchangeName, opt => opt.MapFrom(src => src.ExchangeName));

            CreateMap<Property, Q1ExpectedPriceType9>()
                .ForMember(dst => dst.GrossPriceAmount, opt => opt.MapFrom(src => src.ExpectedPrice))
                .ForMember(dst => dst.NetPriceAmount, opt => opt.MapFrom(src => src.NetPriceAmount))
                .ForMember(dst => dst.VATAmount, opt => opt.MapFrom(src => src.VATAmount));

            CreateMap<decimal, AmountType15>()
                .ForMember(dst => dst.Value, opt => {
                    opt.Condition(src => src != decimal.Zero);
                    opt.MapFrom(src => src);
                })
                .ForMember(dst => dst.currencyID, opt => opt.MapFrom(src => ""));

            //CreateMap<Contact, Q1ContactType8[]>().ConstructUsing(
            //    x => new Q1ContactType8[1] { Name = nx.Name, Communication = x.Telephone }
            //);

            //CreateMap<Contact, Q1ContactType8>()
            //    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dst => dst.Communication, opt => opt.MapFrom(src => src));
            //CreateMap<Contact, Q1CommunicationType8>()
            //    .ForMember(dst => dst.Telephone, opt => opt.MapFrom(src => src.Telephone));

            CreateMap<string, Q2TextType10>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType11>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType5>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType12>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<DateTime, SearchFromDateType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.ToString("yyyy-MM-dd")));
            CreateMap<string, PriorityCodeType3>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => PriorityCodeType(src)));
        }

        public Q1ContactType8[] Contact(Contact cont)
        {
            if (cont == null)
                return null;

            var contact = new Q1ContactType8[1];

            var con = new Q1ContactType8()
            {
                Name = new Q3TextType11() { Value = cont.Name },
                Communication = new Q1CommunicationType8() { Telephone = new Q3TextType11() { Value = cont.Telephone } }
            };

            contact[0] = con;
            return contact;
        }

        public PriorityCodeContentType3 PriorityCodeType(string type)
        {
            if (type.Equals("Purchase")) return PriorityCodeContentType3.Item10;
            if (type.Equals("Lease")) return PriorityCodeContentType3.Item20;
            if (type.Equals("Charge")) return PriorityCodeContentType3.Item30;
            return PriorityCodeContentType3.Item10;
        }
    }
}
