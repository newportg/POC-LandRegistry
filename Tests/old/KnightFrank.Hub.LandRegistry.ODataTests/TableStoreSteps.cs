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

namespace KnightFrank.Hub.LandRegistry.ODataTests
{
    [Binding]
    public class TableStoreSteps
    {
        private readonly ScenarioContext context;
        private readonly IMapper _mapper;

        public TableStoreSteps(ScenarioContext context)
        {
            this.context = context;
            var services = Bootstrapper();
            this._mapper = services.GetRequiredService<IMapper>();
        }

        [Given(@"A Connection to a TableStore Container (.*) (.*)")]
        public void GivenAConnectionToATableStoreContainer(string storageAcct, string container)
        {
            context.Set(new TableStore(storageAcct, "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==", container), "TableStore");
            context.Set(container, "Container");
        }

        [Given(@"I Insert a TableStore object (.*)")]
        public void GivenIInsertATableStoreObject(string json)
        {
            WhenIInsertATableStoreObject(json);
        }

        [When(@"I Insert a TableStore object (.*)")]
        public void WhenIInsertATableStoreObject(string json)
        {
            var ts = context.Get<TableStore>("TableStore");

            var dto = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var entity = _mapper.Map<LandRegistryEntity>(dto);

            context.Set<IFlurlResponse>(ts.Insert(entity), "Response");
        }

        [When(@"I Delete a TableStore object (.*)")]
        public void WhenIDeleteATableStoreObject(string json)
        {
            var ts = context.Get<TableStore>("TableStore");

            var dto = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var entity = _mapper.Map<LandRegistryEntity>(dto);

            context.Set<IFlurlResponse>(ts.Delete(entity), "Response");
        }

        [When(@"I Get a TableStore object (.*)")]
        public void WhenIGetATableStoreObject(string json)
        {
            var ts = context.Get<TableStore>("TableStore");

            var dto = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var entity = _mapper.Map<LandRegistryEntity>(dto);

            context.Set<dynamic>(ts.Get(entity), "Response");
        }

        [When(@"I Get a List of the available TableStore containers")]
        public void WhenIGetAListOfTheAvailableTableStoreContainers()
        {
            var ts = context.Get<TableStore>("TableStore");
            context.Set<IFlurlResponse>(ts.ListContainers(), "Response");
        }

        [Then(@"The returned TableStore object should be the same as the original Object (.*)")]
        public void ThenTheReturnedTableStoreObjectShouldBeTheSameAsTheOriginalObject(string json)
        {
            var res = context.Get<dynamic>("Response");
            LandRegistryDto obj = JsonConvert.DeserializeObject<LandRegistryDto>(json);
            var dto = new LandRegistryDto
            {
                NewRequest = new NewRequest()
                {
                    AddressId = res.AddressId,
                    PropertyNumber = res.PropertyNumber,
                    PostCode = res.PostCode
                },
                RequestType = obj.RequestType
            };

            dto.Equals(obj).Should().BeTrue();
        }

        [AfterScenario("TableStore1", "TableStore2", "TableStore3")]
        public void AfterScenario()
        {
            var ts = context.Get<TableStore>("TableStore");
            ts.DeleteContainer();
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
