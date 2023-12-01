using Azure;
using Repro.Api.Function.Constants;
using Repro.Api.Function.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Repro.Api.Function.Functions;

public class RequestHandler
{
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RequestHandler>();
    }

    [Function(nameof(RequestHandler))]
    [OpenApiOperation(
        operationId: nameof(RequestHandler),
        Summary = "Get data generically",
        Description = "Get data generically",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: FunctionConstants.JsonContentType,
        bodyType: typeof(ResponseJson),
        Summary = "ResponseJson",
        Description = "ResponseJson")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.NotFound,
        contentType: FunctionConstants.JsonContentType,
        bodyType: typeof(ProblemDetails),
        Summary = "No data found",
        Description = "Data could not be found")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.InternalServerError,
        contentType: FunctionConstants.JsonContentType,
        bodyType: typeof(ProblemDetails),
        Summary = "Error while retrieving the data",
        Description = "Error while attempting to retrieve the data")]
    public HttpResponseData Handle(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{name}")]
        HttpRequestData request,
        string name)
    {
        var response = request.CreateResponse();
        response.Headers.Add("Content-Type", "application/json");

        try
        {
            var responseJson = new ResponseJson(name);
            var responseData =  JsonConvert.SerializeObject(responseJson);

            response.StatusCode = HttpStatusCode.OK;
            response.WriteString(responseData);
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Call for {name} failed", name);

            var problemdetails = new ProblemDetails
            {
                Title = "No data found",
                Instance = request.Url.AbsolutePath,
                Status = (int)HttpStatusCode.NotFound,
                Detail = $"No data found for call '{name}'. Are you sure this is a valid path name ?"
            };

            var result = JsonConvert.SerializeObject(problemdetails);
            response.WriteString(result);
            response.StatusCode = HttpStatusCode.NotFound;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Call for {name} failed", name);

            var problemdetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Instance = request.Url.AbsolutePath,
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = $"Internal Server Error for call '{name}'. Please contact our user support desk."
            };

            var result = JsonConvert.SerializeObject(problemdetails);
            response.WriteString(result);
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }
}
