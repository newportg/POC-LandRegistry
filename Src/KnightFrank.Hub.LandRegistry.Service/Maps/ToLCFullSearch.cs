using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToLCFullSearch : Profile
    {
        public ToLCFullSearch()
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestLandChargesFullSearchV2_1Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType7>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType6>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType6>()       
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.CustomerReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.ExpectedPrice, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.Contact, opt => opt.MapFrom(src => src.Contact))
                .ForMember(dst => dst.LandChargesFullSearch, opt => opt.MapFrom(src => src.SearchParty))
                .ForMember(dst => dst.AlternativeDespatchDetails, opt => opt.MapFrom(src => src.AlternativeDespatchDetails));

            CreateMap<Identity, Q1ExternalReferenceType11>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Identity, Q1CustomerReferenceType4>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<Property, Q1ExpectedPriceType3>()
                .ForMember(dst => dst.GrossPriceAmount, opt => opt.MapFrom(src => src.ExpectedPrice))
                .ForMember(dst => dst.NetPriceAmount, opt => opt.MapFrom(src => src.NetPriceAmount))
                .ForMember(dst => dst.VATAmount, opt => opt.MapFrom(src => src.VATAmount));

            CreateMap<decimal, AmountType5>()
                .ForMember(dst => dst.Value, opt => {
                    opt.Condition(src => src != decimal.Zero);
                    opt.MapFrom(src => src);
                })
                .ForMember(dst => dst.currencyID, opt => opt.MapFrom(src => ""));

            CreateMap<Contact, Q1ContactType3>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Communication, opt => opt.MapFrom(src => src));
            CreateMap<Contact, Q1CommunicationType3>()
                .ForMember(dst => dst.Telephone, opt => opt.MapFrom(src => src.Telephone));

            CreateMap<SearchParty, Q1LandChargesFullSearchType1>()
                .ForMember(dst => dst.ContinueIfActualFeeExceedsExpectedFeeIndicator, opt => opt.MapFrom(src => src.ContinueIfFeeExceeds))
                .ForMember(dst => dst.CountyOrAdministrativeArea, opt => opt.MapFrom(src => src.County))
                .ForMember(dst => dst.Items, opt => opt.MapFrom(src => Item(src)));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchDetailsType3>()
                .ForMember(dst => dst.AlternativeDespatchReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.AlternativeDespatchName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.AlternativeDespatchAddress, opt => opt.MapFrom(src => src));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchAddressType1>()
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => src));
            CreateMap<AlternativeDespatchDetails, Q1AlternativePostalAddressType3>()
                .ForMember(dst => dst.AddressLine, opt => opt.MapFrom(src => src.Address))
                .ForMember(dst => dst.Postcode, opt => opt.MapFrom(src => src.PostCode));
            CreateMap<AlternativeDespatchDetails, Q1DXDetailsType3>()
                .ForMember(dst => dst.DXNumber, opt => opt.MapFrom(src => src.DXNumber))
                .ForMember(dst => dst.ExchangeName, opt => opt.MapFrom(src => src.ExchangeName));

            CreateMap<string, Q3TextType5>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));

            //CreateMap<string, Q3TextType1>()
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            //CreateMap<string, Q3TextType2>()
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType5>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
        }

        public static object Item(SearchParty parties)
        {
            if (parties == null || parties.Applicants == null)
                return null;

            if (parties.SearchType == SearchType.Individual || parties.SearchType == SearchType.Company)
            {
                object[] sp = new Q1StandardNamesToSearchType1[parties.Applicants.Length];
                for (var i = 0; i < parties.Applicants.Length; i++)
                {
                    var search = new Q1StandardNamesToSearchType1
                    {
                        Name = new StandardNameType1()
                    };
                    if (parties.SearchType == SearchType.Individual)
                    {
                        search.Name.Item = new IndividualNameType1()
                        {
                            Forename = parties.Applicants[i].Forename,
                            Surname = parties.Applicants[i].Surname,
                        };
                    }
                    else
                    {
                        search.Name.Item = new String(parties.Applicants[i].CompanyName);
                    }
                    search.ToYear = new Q3TextType5() { Value = parties.Applicants[i].To.ToString("yyyy") };
                    search.FromYear = new Q3TextType5() { Value = parties.Applicants[i].From.ToString("yyyy") };

                    sp[i] = search;
                }
                return sp;
            }
            else if (parties.SearchType == SearchType.Complex || parties.SearchType == SearchType.LocalAuthority)
            {
                object[] sp = new Q1NonStandardNamesToSearchType1[parties.Applicants.Length];
                for (var i = 0; i < parties.Applicants.Length; i++)
                {
                    var search = new Q1NonStandardNamesToSearchType1
                    {
                        Name = parties.Applicants[i].ComplexName,
                        ToYear = new Q3TextType5() { Value = parties.Applicants[i].To.ToString("yyyy") },
                        FromYear = new Q3TextType5() { Value = parties.Applicants[i].From.ToString("yyyy") }
                    };

                    sp[i] = search;
                }
                return sp;
            }
            return null;
        }
    }
}
