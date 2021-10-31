using System;
using System.Net;

namespace Pokedex.Api.Exceptions
{
    [Serializable]
    public class ExternalApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public ExternalApiException()
        {
        }

        public ExternalApiException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public ExternalApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}