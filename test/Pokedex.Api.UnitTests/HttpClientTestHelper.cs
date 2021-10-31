using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Pokedex.Api.UnitTests
{
    public static class HttpClientTestHelper
    {
        public static HttpClient SetupHttpClient(HttpStatusCode httpStatusCode, StringContent data = null)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = data
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://someMockUrl:80")
            };
        }
    }
}
