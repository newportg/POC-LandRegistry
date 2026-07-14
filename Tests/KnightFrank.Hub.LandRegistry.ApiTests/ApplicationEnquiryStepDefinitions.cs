using System;
using Reqnroll;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;
using Flurl.Http;
using KnightFrank.Hub.LandRegistry.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using KnightFrank.Hub.LandRegistry.ApiTests;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace KnightFrank.Hub.LandRegistry.ApiTests.StepDefinitions
{
    [Binding]
    public class ApplicationEnquiryStepDefinitions
    {
        private readonly ScenarioContext context;

        public ApplicationEnquiryStepDefinitions(ScenarioContext context)
        {
            this.context = context;
            var services = Bootstrapper();
        }

        [Given(@"UniqueMsgId (.*)")]
        public void GivenUniqueMsgId(string param)
        {
            context.Set(param, "UniqueMsgId");
        }

        [Given(@"TitleNumber (.*)")]
        public void GivenTitleNumber(string param)
        {
            context.Set(param, "TitleNumber");
        }

        [Given(@"ClosedAndContinued Flag (.*)")]
        public void GivenClosedAndContinuedFlagTrue(bool param)
        {
            context.Set(param, "ClosedAndContinued");
        }

        [Given(@"ContinueIfFeeExceeds Flag (.*)")]
        public void GivenContinueIfFeeExceedsFlag(bool param)
        {
            context.Set(param, "ContinueIfFeeExceeds");
        }

        [Given(@"ApplicationReference (.*)")]
        public void GivenApplicationReference(string param)
        {
            context.Set(param, "ApplicationReference");
        }

        [When(@"I call the land registy API")]
        public void WhenICallTheLandRegistyAPI()
        {
            var appEnq = new ApplicationEnquiryReq()
            {
                Identity = new Common.Models.Client.Identity()
                {
                    UniqueMsgId = context.Get<string>("UniqueMsgId")
                },
                Property = new ApplicationEnquiryReq_Property()
                {
                    TitleNumber = context.Get<string>("TitleNumber"),
                    ContinueIfTitleIsClosedAndContinuedIndicator = context.Get<bool>("ClosedAndContinued")
                },
                ApplicationReference = context.Get<string>("ApplicationReference")
            };


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(appEnq);
            Console.WriteLine(json);

            DirectoryInfo dir = new (System.IO.Directory.GetCurrentDirectory());
            Console.WriteLine(dir.FullName);
            DirectoryInfo projectDirectory = new(System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\src\API\KnightFrank.Hub.LandRegistry.Api\")));

            var afr = AzureFunctionRunner.StartNewAsync(projectDirectory).Result;

            var result = $"http://localhost:{Environment.GetEnvironmentVariable("FunctionPortNo")}/api/ApplicationEnquiry"
                .PostJsonAsync(appEnq)
                .ReceiveString().Result;

            var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<AE_LandRegistryDto>(result);

            afr.DisposeAsync();

            context.Set(dto, "LandRegistryDto");
        }

        [Then(@"I should recieve a LandregistryDTO (.*)")]
        public void ThenIShouldRecieveALandregistryDTOResponse(string param)
        {
            var result = context.Get<AE_LandRegistryDto>("LandRegistryDto");
            result.Should().NotBeNull();

            var json = JsonConvert.SerializeObject(result);
            Console.WriteLine($"TestResponse    : {json}");
        }

        private static ServiceProvider Bootstrapper()
        {
            Environment.SetEnvironmentVariable("FunctionPortNo", "7076");
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

            var services = new ServiceCollection();
            return services.BuildServiceProvider();
        }
    }

    public class AE_LandRegistryDto
    {
        public RequestTypes RequestType { get; set; }
        public Request Request { get; set; }
        public AE_Response Response { get; set; }
        public Error SystemError { get; set; }
    }

    public class AE_Response
    {
        public string Status { get; set; }
        public Acknowledgement Acknowledgement { get; set; }
        public Rejection Rejection { get; set; }
        public AE_Results Results { get; set; }
    }

    public class AE_Results
    {
        public string ExternalReference { get; set; }
        //public ApplicationEnquiryMessageDetails MessageDetails { get; set; }
        public MessageMessageDetails MessageDetails { get; set; }
    }
}
