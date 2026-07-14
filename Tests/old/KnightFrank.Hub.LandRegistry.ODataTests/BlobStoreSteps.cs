using AutoMapper;
using FluentAssertions;
using Flurl.Http;
using KnightFrank.Hub.LandRegistry.Common;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.OData;
using KnightFrank.Hub.LandRegistry.OData.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace Library.Azure.OdataTests
{
    [Binding]
    public class BlobStoreSteps
    {
        private readonly ScenarioContext context;
        private readonly IMapper _mapper;

        public BlobStoreSteps(ScenarioContext context)
        {
            this.context = context;
            var services = Bootstrapper();
            this._mapper = services.GetRequiredService<IMapper>();
        }

        [Given(@"A Connection to a BlobStore Container (.*) (.*)")]
        public void GivenAConnectionToABlobStoreContainer(string storageAcct, string container)
        {
            context.Set(new BlobStore(storageAcct, "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==", container), "BlobStore");
            context.Set(container, "Container");
        }

        [Given(@"I Insert a object (.*)")]
        public void GivenIInsertAObject(string json)
        {
            WhenIInsertAObject(json);
        }

        [When(@"I Insert a object (.*)")]
        public void WhenIInsertAObject(string json)
        {
            var bs = context.Get<BlobStore>("BlobStore");

            var dto = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var entity = _mapper.Map<LandRegistryEntity>(dto);

            context.Set<IFlurlResponse>(bs.Insert(entity), "Response");
        }

        [When(@"I Get a object (.*)")]
        public void WhenIGetAObject(string json)
        {
            var bs = context.Get<BlobStore>("BlobStore");

            var dto = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var entity = _mapper.Map<LandRegistryEntity>(dto);

            context.Set<dynamic>(bs.Get(entity), "Response");
        }

        [When(@"I Delete a object (.*)")]
        public void WhenIDeleteAObject(string json)
        {
            var bs = context.Get<BlobStore>("BlobStore");

            var dto = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var entity = _mapper.Map<LandRegistryEntity>(dto);

            context.Set<IFlurlResponse>(bs.Delete(entity), "Response");
        }

        [When(@"I Get a List of the available containers")]
        public void WhenIGetAListOfTheAvailableContainers()
        {
            var bs = context.Get<BlobStore>("BlobStore");
            context.Set<IFlurlResponse>(bs.ListContainers(), "Response");
        }

        [Then(@"I should receive a HttpStatus (.*)")]
        public void ThenIShouldReceiveAHttpStatus(string status)
        {
            var res = context.Get<IFlurlResponse>("Response");
            res.StatusCode.Should().Be(int.Parse(status));
        }

        [Then(@"The returned blob should be the same as the original Object (.*)")]
        public void ThenTheReturnedBlobShouldBeTheSameAsTheOriginalObject(string json)
        {
            var res = context.Get<dynamic>("Response");
            LandRegistryDto obj = JsonConvert.DeserializeObject<LandRegistryDto>(json);

            var dto = new LandRegistryDto
            {
                NewRequest = new NewRequest()
                {
                    UniqueMsgId = res.UniqueMsgId,
                    AddressId = res.AddressId,
                    PropertyName = res.PropertyName,
                    PropertyNumber = res.PropertyNumber,
                    Line1 = res.Line1,
                    Line2 = res.Line2,
                    Line3 = res.Line3,
                    City = res.City,
                    County = res.County,
                    PostCode = res.PostCode
                },
                RequestType = obj.RequestType
            };

            dto.Equals(obj).Should().BeTrue();
        }

        [Then(@"The returned list will not contain (.*)")]
        public void ThenTheReturnedListWillNotContain(string p0)
        {
            var res = context.Get<IFlurlResponse>("Response");
        }

        [AfterScenario("BlobStore1", "BlobStore2", "BlobStore3")]
        public void AfterScenario()
        {
            var bs = context.Get<BlobStore>("BlobStore");
            bs.DeleteContainer();
        }

        private static ServiceProvider Bootstrapper()
        {
            var services = new ServiceCollection();

            services.AddLogging(config => config.AddConsole());
            services.AddCoreServices();
            services.AddODataServices("http://127.0.0.1:10002/devstoreaccount1", "eed0kJ5uDqBWPNkqW4g%2F8YaL1%2BiRuGuqF6GZM2QLa38%3D", "LandRegistry");

            return services.BuildServiceProvider();
        }
    }
}
