using FluentAssertions;
using Flurl;
using Flurl.Http;
using KnghtFrank.Hub.LandRegistry.Subscribers;
using KnightFrank.Hub.LandRegistry.Common.Query;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace KnghtFrank.Hub.LandRegistry.SubscribersTests
{
    [Binding]
    public class FindTitleSteps
    {
        [BeforeTestRun(Order = 1)]
        public static void Before()
        {
            FunctionHostManager.StartHost();
        }

        [AfterTestRun]
        public static void After()
        {
            FunctionHostManager.StopHost();
        }

        private readonly ScenarioContext context;

        public FindTitleSteps(ScenarioContext context, WebApplicationFactory<Startup> webApplicationFactory)
        {
            this.context = context;
            _ = webApplicationFactory;
        }

        [Given(@"I have the following request body:")]
        public void GivenIHaveTheFollowingRequestBody(string json)
        {
            // add the request into the scenario context for later use
            context.Set(json, "Request");
        }

        [When(@"I post this request to the ""(.*)"" Operation")]
        public async Task WhenIPostThisRequestToTheOperationAsync(string operation)
        {
            // retrieve request
            var requestBody = context.Get<string>("Request");
            var _url = "http://localhost:7071/api/"
                .AppendPathSegment($"/{operation}");

            var response = await _url.PostJsonAsync(JsonConvert.DeserializeObject<FindProperty>(requestBody));
            try
            {
                context.Set(response.StatusCode, "ResponseStatusCode");
                context.Set(response.ResponseMessage.ReasonPhrase, "ResponseReasonPhrase");
                var responseBody = await response.GetStringAsync().ConfigureAwait(false);
                context.Set(responseBody, "ResponseBody");
            }
            finally
            {
                // move along, move along
            }
        }

        [Then(@"The result should be a (.*) \(""(.*)""\) response")]
        public void ThenTheResultShouldBeAResponse(HttpStatusCode statusCode, string reasonPhrase)
        {
            context.Get<HttpStatusCode>("ResponseStatusCode").Should().Be(statusCode);
            context.Get<string>("ResponseReasonPhrase").Should().Be(reasonPhrase);
        }
    }

}
