using FluentAssertions;
using KnightFrank.Hub.LandRegistry.Common;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Common.Query;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace KnightFrank.Hub.LandRegistry.CommonTests
{
    [Binding]
    public class FindPropertySteps
    {
        private readonly ScenarioContext context;
        private readonly IMediator _mediator;

        public FindPropertySteps(ScenarioContext context) //, WebApplicationFactory<Startup> webApplicationFactory)
        {
            this.context = context;
            //_ = webApplicationFactory;
            var services = Bootstrapper();
            _mediator = services.GetRequiredService<IMediator>();
        }

        [Given(@"For a given property detail (.*)")]
        public void GivenForAGivenPropertyDetail(string json)
        {
            context.Set(json, "Request");
        }

        [When(@"I call the land registy to (.*)")]
        public async Task WhenICallTheLandRegistyTo(string p0)
        {
            try
            {
                var request = context.Get<string>("Request");

                var obj = JsonConvert.DeserializeObject<FindProperty>(request);

                var response = await _mediator.Send(obj);
                context.Set(response, "Response");
            }
            finally
            {
                // move along, move along
            }
        }

        [Then(@"I should receive (.*)")]
        public void ThenIShouldReceive(string response)
        {
            context.Get<FindResponse>("Response").Should().Be(response);
        }

        private ServiceProvider Bootstrapper()
        {
            var services = new ServiceCollection();

            //var tableStorageUrl = "http://127.0.0.1:10002/devstoreaccount1";
            //var tableStorageSasKey = "gjQA2Dm%2FR4bGWDMkX%2B85ufBGvb2G9Uwss6GLUzAc6Zg%3D";
            //var tableStorageTableName = "LandRegistry";

            services.AddLogging(config => config.AddConsole());
            services.AddCoreServices();
            //services.AddODataServices(tableStorageUrl, tableStorageSasKey, tableStorageTableName);

            return services.BuildServiceProvider();
        }
    }
}
