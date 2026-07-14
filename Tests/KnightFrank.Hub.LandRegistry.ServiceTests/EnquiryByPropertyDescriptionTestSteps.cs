using FluentAssertions;
using KnightFrank.Hub.LandRegistry.Common;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using TechTalk.SpecFlow;

namespace KnightFrank.Hub.LandRegistry.ServiceTests
{
    [Binding]
    public class EnquiryByPropertyDescriptionTestSteps
    {
        private readonly ScenarioContext context;
        private readonly ILandRegistrySvc _hmlr;

        public EnquiryByPropertyDescriptionTestSteps(ScenarioContext context)
        {
            this.context = context;
            var services = Bootstrapper();
            _hmlr = services.GetRequiredService<ILandRegistrySvc>();
        }

        [Given(@"For a given property detail (.*)")]
        public void GivenForAGivenPropertyDetail(string json)
        {
            context.Set(json, "Request");
        }

        [Given(@"The Expected Message Details will be (.*)")]
        public void GivenTheExpectedMessageDetailsWillBe(string json)
        {
            context.Set(json, "ResponseMessageDetails");
        }

        [Given(@"A different (.*) and (.*)")]
        public void GivenADifferentAndAnd(string username, string password)
        {
            Environment.SetEnvironmentVariable("LandRegistry_UserId", username);
            Environment.SetEnvironmentVariable("LandRegistry_Password", password);
            context.Set("{\"AddressId\":\"AddressId\", \"PropertyNumber\": \"99\", \"PostCode\":\"TQ56 4HY\"}", "Json");
        }

        [Given(@"A expected price of (.*)")]
        public static void GivenAExpectedPriceOf(string expectedPrice)
        {
            Environment.SetEnvironmentVariable("LandRegistry_ExpectedPrice", expectedPrice);
        }

        [Given(@"If Fee exceeds the expected price (.*)")]
        public static void GivenIfFeeExceedsTheExpectedPrice(string feeExceedPrice)
        {
            Environment.SetEnvironmentVariable("LandRegistry_ContinueIfFeeExceedsExpectedPrice", feeExceedPrice);
        }

        [When(@"I call the land registy (.*)")]
        public void WhenICallTheLandRegisty(RequestTypes request)
        {
            try
            {
                var json = context.Get<string>("Request");

                var obj = LandRegistryDto.GetRequest(request, json);
                //if (obj == null)
                //{
                //    // Set the Request Type
                //    obj = new LandRegistryDto
                //    {
                //        RequestType = request
                //    };

                //    if (!string.IsNullOrEmpty(json))
                //    {
                //        var req = JsonConvert.DeserializeObject<Request>(json);
                //        obj.Request = req;
                //    }
                //}

                var response = _hmlr.FindProperty(obj).Result;

                context.Set(response, "Response");
            }
            finally
            {
                // move along, move along
            }
        }

        [Then(@"I should receive (.*)")]
        public void ThenIShouldReceive(string expectedResponseJson)
        {
            // Get the returned object from the Service
            var serviceResponseObj = context.Get<LandRegistryDto>("Response");

            var json = JsonConvert.SerializeObject(serviceResponseObj);
            Console.WriteLine($"LR ServiceResponse : {json}");

            // Create a synthetic LR Object 
            var testObj = CreateSyntheticLRObject(serviceResponseObj.RequestType, expectedResponseJson);

            if (testObj.Response != null)
            {
                if (serviceResponseObj.Response.Status.Equals("Acknowledgement"))
                {
                    serviceResponseObj.Response.Acknowledgement.Equals(testObj.Response.Acknowledgement).Should().BeTrue();
                }
                else if (serviceResponseObj.Response.Status.Equals("Success"))
                {
                    serviceResponseObj.Response.Results.Equals(testObj.Response.Results).Should().BeTrue();
                }
                else if (serviceResponseObj.Response.Status.Equals("Rejection"))
                {
                    serviceResponseObj.Response.Rejection.Equals(testObj.Response.Rejection).Should().BeTrue();
                }
            }
            else
            {
                serviceResponseObj.SystemError.Equals(testObj.SystemError).Should().BeTrue();
            }

        }

        [Then(@"ExpectedResponseDateTime should be present (.*)")]
        public void ThenExpectedResponseDateTimeShouldBePresent(string present)
        {
            var serviceResponseObj = context.Get<LandRegistryDto>("Response");
            if (present.ToLower().Equals("true"))
            {
                //serviceResponse.Response.Acknowledgement.ExpectedResponseDateTime.Should().Be(default(DateTime));
                serviceResponseObj.Response.Acknowledgement.ExpectedResponseDateTime.Should().BeOnOrAfter(DateTime.Now);
            }
        }

        private LandRegistryDto CreateSyntheticLRObject( RequestTypes rqType, string expectedResponseJson)
        {
            // Create a synthetic LR Object 

            var jsonRequest = context.Get<string>("Request");
            var testRequestObj = JsonConvert.DeserializeObject<Request>(jsonRequest);
            var testResponseObj = JsonConvert.DeserializeObject<LandRegistryDto>(expectedResponseJson);
            var testMessageDetailsObj = context.Get<string>("ResponseMessageDetails");

            if (!string.IsNullOrEmpty(testMessageDetailsObj))
            {
                var md = CreateMessageDetails(rqType, testMessageDetailsObj);
                testResponseObj.Response.Results.MessageDetails = md;
            }

            testResponseObj.RequestType = rqType;
            testResponseObj.Request = testRequestObj; // Assign request from service response, not testing the request

            var json = JsonConvert.SerializeObject(testResponseObj);
            Console.WriteLine($"Expected Response  : {json}");

            return testResponseObj;
        }

        private static ServiceProvider Bootstrapper()
        {
            var services = new ServiceCollection();


            Environment.SetEnvironmentVariable("CertName", "FreddoFrog"); 
            Environment.SetEnvironmentVariable("KeyVaultUri", "https://kv-tst-landr-vse-ne.vault.azure.net/");


            Environment.SetEnvironmentVariable("LandRegistryCertificates", "B10D6788259CA89F7309A07C334B1B2DE4B7D520");
            Environment.SetEnvironmentVariable("LandRegistryUserId", "BGUser001");
            Environment.SetEnvironmentVariable("LandRegistryPassword", "landreg001");

            // Test 
            Environment.SetEnvironmentVariable("LandRegistryBaseAddress", "https://bgtest.landregistry.gov.uk/");

            Environment.SetEnvironmentVariable("LandRegistryApplicationEnquiry", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0WebService");
            //Environment.SetEnvironmentVariable("LandRegistryLCBankruptcySearch", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_0WebService");
            Environment.SetEnvironmentVariable("LandRegistryLCBankruptcySearch", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_1WebService");
            Environment.SetEnvironmentVariable("LandRegistryDischargeActivity", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/DischargeActivityV1_0WebService");
            Environment.SetEnvironmentVariable("LandRegistryEnquiryByPropertyDescription", "https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/EnquiryByPropertyDescriptionV2_0WebService");
            Environment.SetEnvironmentVariable("LandRegistryLCFullSearch", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_1WebService");
            Environment.SetEnvironmentVariable("LandRegistryOfficialCopyTitleKnown", "https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialCopyTitleKnownV2_1WebService");
            Environment.SetEnvironmentVariable("LandRegistryOfficialSearchWhole", "https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_1WebService");
            Environment.SetEnvironmentVariable("LandRegistryOfficialSearchPart", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_1WebService");

            Environment.SetEnvironmentVariable("LandRegistryPollApplicationEnquiry", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0PollRequestWebService");
            Environment.SetEnvironmentVariable("LandRegistryPollLCBankruptcySearch", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_0PollRequestWebService");
            Environment.SetEnvironmentVariable("LandRegistryPollDischargeActivity", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/DischargeActivityV1_0PollRequestWebService");
            Environment.SetEnvironmentVariable("LandRegistryPollEnquiryByPropertyDescription", "https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/EnquiryByPropertyDescriptionV2_0PollWebService");
            Environment.SetEnvironmentVariable("LandRegistryPollLCFullSearch", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_1PollRequestWebService");
            Environment.SetEnvironmentVariable("LandRegistryPollOfficialSearchWhole", "https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_0PollRequestWebService");
            Environment.SetEnvironmentVariable("LandRegistryPollOfficialSearchPart", "https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_1PollRequestWebService");

            Environment.SetEnvironmentVariable("LandRegistryExpectedPrice", "3");
            Environment.SetEnvironmentVariable("LandRegistryContinueIfFeeExceedsExpectedPrice", "false");
            Environment.SetEnvironmentVariable("LandRegistryContactName", "Knight Frank");
            Environment.SetEnvironmentVariable("LandRegistryContactPhone", "01234 5678901");

            Environment.SetEnvironmentVariable("LandRegistryBGVendor", "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\r\n");
            Environment.SetEnvironmentVariable("LandRegistry2020IssuingCA", "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\r\n");
            Environment.SetEnvironmentVariable("LandRegistryRootCA", "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\r\n");

            services.AddLogging(config => config.AddConsole());
            services.AddCoreServices();
            services.AddLandRegistryServices();
            return services.BuildServiceProvider();
        }

        private static MessageDetails CreateMessageDetails(RequestTypes type, string json)
        {
            switch (type)
            {
                case RequestTypes.ApplicationEnquiry:
                    return JsonConvert.DeserializeObject<ApplicationEnquiryMessageDetails>(json);
                case RequestTypes.LCBankruptcySearch:
                    return JsonConvert.DeserializeObject<LCBankruptcySearchMessageDetails>(json);
                case RequestTypes.DischargeActivity:
                    return JsonConvert.DeserializeObject<DischargeActivityMessageDetails>(json);
                case RequestTypes.EnquiryByPropertyDescription:
                    return JsonConvert.DeserializeObject<EnquiryByPropertyDescriptionMessageDetails>(json);
                case RequestTypes.LCFullSearch:
                    return JsonConvert.DeserializeObject<LCFullSearchMessageDetails>(json);
                case RequestTypes.OfficialCopyTitleKnown:
                    return JsonConvert.DeserializeObject<TitleOfficalCopyMessageDetails>(json);
                case RequestTypes.OfficialSearchWhole:
                    return JsonConvert.DeserializeObject<OfficialSearchWholeMessageDetails>(json);
                case RequestTypes.OfficialSearchPart:
                    return JsonConvert.DeserializeObject<OfficialSearchPartMessageDetails>(json);

                case RequestTypes.PollApplicationEnquiry:
                    return JsonConvert.DeserializeObject<ApplicationEnquiryMessageDetails>(json);
                case RequestTypes.PollLCBankruptcySearch:
                    return JsonConvert.DeserializeObject<LCBankruptcySearchMessageDetails>(json);
                case RequestTypes.PollDischargeActivity:
                    return JsonConvert.DeserializeObject<DischargeActivityMessageDetails>(json);
                case RequestTypes.PollLCFullSearch:
                    return JsonConvert.DeserializeObject<LCFullSearchMessageDetails>(json);
                case RequestTypes.PollEnquiryByPropertyDescription:
                    return JsonConvert.DeserializeObject<EnquiryByPropertyDescriptionMessageDetails>(json);
                case RequestTypes.PollOfficialSearchWhole:
                    return JsonConvert.DeserializeObject<OfficialSearchWholeMessageDetails>(json);
                case RequestTypes.PollOfficialSearchPart:
                    return JsonConvert.DeserializeObject<OfficialSearchPartMessageDetails>(json);
                default:
                    break;
            }
            return null;
        }
    }
}
