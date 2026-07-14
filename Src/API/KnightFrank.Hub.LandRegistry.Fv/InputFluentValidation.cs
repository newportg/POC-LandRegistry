using FluentValidation;
using KnightFrank.Hub.LandRegistry.Common.Models.Client.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace KnightFrank.Hub.LandRegistry.Fv
{
    public class InputFluentValidation(ILogger<InputFluentValidation> log, IValidator<SampleInput> validator)
    {
        private readonly ILogger _logger = log;
        private readonly IValidator<SampleInput> _validator = validator;

        [Function("InputFluentValidation")]
        [OpenApiOperation(operationId: "Function", tags: new[] { "sample" }, Summary = "Sample input validation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SampleInput), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Summary = "Invalid input supplied")]
        public async Task<HttpResponseData> Function([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var input = await req.ReadFromJsonAsync<SampleInput>();
                if (input is null)
                {
                    var error = req.CreateResponse(HttpStatusCode.BadRequest);
                    await error.WriteAsJsonAsync(
                        new ProblemDetails
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Title = "Missing body"
                        });

                    return error;
                }

                var validationResult = await _validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    var error = req.CreateResponse(HttpStatusCode.BadRequest);
                    await error.WriteAsJsonAsync(
                        new ProblemDetails
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Title = "One or more validation errors occurred.",
                            Extensions = {
                                ["errors"] = validationResult.Errors.Select(e => new
                                    {
                                        e.ErrorCode,
                                        e.PropertyName,
                                        e.ErrorMessage
                                    })
                            }
                        });

                    return error;
                }

                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch(Newtonsoft.Json.JsonReaderException ex) 
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteAsJsonAsync(
                    new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Title = $"Invalid Json at line {ex.LineNumber} position {ex.LinePosition}"
                    });

                return error;
            }
        }
    }
}
