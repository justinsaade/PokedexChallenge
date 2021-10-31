using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Exceptions
{
    /*
     * TO:DO Refactor the error mapping out of here.
     */
    public class ErrorHandlerMiddleware
    {
        private const string GeneralErrorType = "GENERAL";
        private const string ExternalErrorType = "EXTERNAL";

        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    HttpRequestException e => (int)e.StatusCode,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                string result = error switch
                {
                    HttpRequestException e => SerialiseMessage(ExternalErrorType, error?.Message),
                    _ => SerialiseMessage(GeneralErrorType, error?.Message),
                };

                await response.WriteAsync(result);
            }
        }

        private string SerialiseMessage(string type, string message)
        {
            return JsonSerializer.Serialize(new { errorType = type, message = message });
        }
    }
}
