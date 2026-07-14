using AutoMapper;
using Azure;
using KnightFrank.Hub.LandRegistry.Common.Models;
using ServiceReference;
using System.Numerics;

namespace KnightFrank.Hub.LandRegistry.Service.Maps
{
    public class ToLCBankruptcySearch : Profile
    {
        public ToLCBankruptcySearch()
        {
            // Custom Maps
            CreateMap<LandRegistryDto, RequestLandChargesBankruptcySearchV2_1Type>()
                .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Request))
                .ForMember(dst => dst.Product, opt => opt.MapFrom(src => src.Request));

            CreateMap<Request, Q1IdentifierType3>()
                .ForMember(dst => dst.MessageID, opt => opt.MapFrom(src => src.Reference));
            CreateMap<Identity, Q1TextType2>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.UniqueMsgId));

            CreateMap<Request, Q1ProductType2>()
                .ForMember(dst => dst.ExternalReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.CustomerReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.ExpectedPrice, opt => opt.MapFrom(src => src.Property))
                .ForMember(dst => dst.Contact, opt => opt.MapFrom(src =>  src.Contact))
                .ForMember(dst => dst.LandChargesBankruptcySearch, opt => opt.MapFrom(src => src.SearchParty))
                .ForMember(dst => dst.AlternativeDespatchDetails, opt => opt.MapFrom(src => src.AlternativeDespatchDetails));

            CreateMap<Identity, Q1ExternalReferenceType4>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.ExternalRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.MapFrom(src => src.AllocatedBy))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Identity, Q1CustomerReferenceType1>()
                .ForMember(dst => dst.Reference, opt => opt.MapFrom(src => src.CustomerRef))
                .ForMember(dst => dst.AllocatedBy, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.Ignore());

            CreateMap<SearchParty, Q1LandChargesBankruptcySearchType1>()
                .ForMember(dst => dst.ContinueIfActualFeeExceedsExpectedFeeIndicator, opt => opt.MapFrom(src => src.ContinueIfFeeExceeds))
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => Item(src)));
                //.ForMember(dst => dst.Item, opt => opt.MapFrom(src => src.ComplexName))
                //.ForMember(dst => dst.Item, opt => opt.MapFrom(src => src.PrivateIndividual));

            //CreateMap<SearchParty, Q1BankruptcySearchComplexNameType1>()
            //    .ForMember(dst => dst.BankruptcySearchParty, opt => opt.MapFrom(src => src));
            //CreateMap<SearchParty, Q1BankruptcySearchComplexNamePartyType1>()
            //    .ForMember(dst => dst.ComplexName, opt => opt.MapFrom(src => src.ComplexName));
            //CreateMap<string, ComplexNameTextType1>()
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));

            //CreateMap<SearchParty, Q1BankruptcySearchPrivateIndividualType1>()
            //    .ForMember(dst => dst.BankruptcySearchParty, opt => opt.MapFrom(src => src.PrivateIndividual));
            //CreateMap<PrivateIndividual, Q1BankruptcySearchPrivateIndividualPartyType1>()
            //    .ForMember(dst => dst.SurnameName, opt => opt.MapFrom(src => src.Surname))
            //    .ForMember(dst => dst.ForenamesName, opt => opt.MapFrom(src => src.Forename));
            //CreateMap<string, Q1ForenamesTextType1>()
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            //CreateMap<string, Q1SurnameTextType1>()
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));

            CreateMap<Property, Q1ExpectedPriceType1>()
                .ForMember(dst => dst.GrossPriceAmount, opt => opt.MapFrom(src => src.ExpectedPrice))
                .ForMember(dst => dst.NetPriceAmount, opt => opt.MapFrom(src => src.NetPriceAmount))
                .ForMember(dst => dst.VATAmount, opt => opt.MapFrom(src => src.VATAmount));

            CreateMap<decimal, AmountType2>()
                .ForMember(dst => dst.Value, opt => {
                    opt.Condition(src => src != decimal.Zero);
                    opt.MapFrom(src => src);
                })
                .ForMember(dst => dst.currencyID, opt => opt.MapFrom(src => ""));

            CreateMap<Contact, Q1ContactType1>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Communication, opt => opt.MapFrom(src => src));
            CreateMap<Contact, Q1CommunicationType1>()
                .ForMember(dst => dst.Telephone, opt => opt.MapFrom(src => src.Telephone));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchDetailsType1>()
                .ForMember(dst => dst.AlternativeDespatchReference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dst => dst.AlternativeDespatchName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.AlternativeDespatchAddress, opt => opt.MapFrom(src => src));

            CreateMap<AlternativeDespatchDetails, Q1AlternativeDespatchAddressType>()
                .ForMember(dst => dst.Item, opt => opt.MapFrom(src => src));
            CreateMap<AlternativeDespatchDetails, Q1AlternativePostalAddressType>()
                .ForMember(dst => dst.AddressLine, opt => opt.MapFrom(src => src.Address))
                .ForMember(dst => dst.Postcode, opt => opt.MapFrom(src => src.PostCode));

            CreateMap<string, Q3TextType>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType1>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<string, Q3TextType2>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
            CreateMap<bool, IndicatorType2>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src));
        }

        public static object Item(SearchParty parties)
        {
            if (parties == null || parties.Applicants == null)
                return null;

            if (parties.SearchType == SearchType.Individual)
            {
                var sp = new Q1BankruptcySearchPrivateIndividualType1
                {
                    BankruptcySearchParty = new Q1BankruptcySearchPrivateIndividualPartyType1[parties.Applicants.Length]
                };

                for (var i = 0; i < parties.Applicants.Length; i++)
                {
                    var eek = new Q1BankruptcySearchPrivateIndividualPartyType1
                    {
                        SurnameName = new Q1SurnameTextType1() { Value = parties.Applicants[i].Surname },
                        ForenamesName = new Q1ForenamesTextType1() { Value = parties.Applicants[i].Forename }
                    };

                    sp.BankruptcySearchParty[i] = eek;
                }

                return sp;
            }
            else if (parties.SearchType == SearchType.Complex || parties.SearchType == SearchType.LocalAuthority)
            {
                var sp = new Q1BankruptcySearchComplexNameType1
                {
                    BankruptcySearchParty = new Q1BankruptcySearchComplexNamePartyType1[parties.Applicants.Length]
                };
                for (var i = 0; i < parties.Applicants.Length; i++)
                {
                    var eek = new Q1BankruptcySearchComplexNamePartyType1
                    {
                        ComplexName = new ComplexNameTextType1() { Value = parties.Applicants[i].ComplexName }
                    };

                    sp.BankruptcySearchParty[i] = eek;
                }
                return sp;
            }

            return null;
        }

    }
}
