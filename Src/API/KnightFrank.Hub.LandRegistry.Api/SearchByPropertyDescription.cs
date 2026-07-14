using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Common.Models.Client;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace KnightFrank.Hub.LandRegistry.Api
{
    public class SearchByPropertyDescription(ILogger<SearchByPropertyDescription> log, IMapper mapper, ILandRegistrySvc hmlr)
    {
        private readonly ILogger<SearchByPropertyDescription> _logger = log;
        private readonly IMapper _mapper = mapper;
        private readonly ILandRegistrySvc _hmlr = hmlr;

        [Function("SearchByPropertyDescription")]
        [OpenApiOperation(operationId: "SearchByPropertyDescriptionReq")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SearchByPropertyDescriptionReq))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LandRegistryDto), Description = "The OK response")]
        public async Task<HttpResponseData> SearchByPropertyDescriptionReq([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("SearchByPropertyDescription request.");
            var response = req.CreateResponse();

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<SearchByPropertyDescriptionReq>(requestBody);

                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(data, new ValidationContext(data), validationResults, validateAllProperties: true);
                if (!isValid)
                {
                    var vresult = ValidationResults.PrintResults(validationResults, 0);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                    response.WriteString(vresult);
                    return response;
                }

                var _request = _mapper.Map<LandRegistryDto>(data);

                var result = _hmlr.FindProperty(_request).Result;
                string responseMessage = JsonConvert.SerializeObject(result);

                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync<LandRegistryDto>(result);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(ex.Message);
                return response;
            }
        }
    }
}
