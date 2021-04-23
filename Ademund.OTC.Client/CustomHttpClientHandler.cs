using Ademund.OTC.Utils;
using Polly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public class CustomHttpClientHandler : SigningHttpClientHandler
    {
        public CustomHttpClientHandler(Signer signer, IWebProxy proxy = null) : base(signer)
        {
            Proxy = proxy;
            UseProxy = proxy != null;
            ServerCertificateCustomValidationCallback += (_, __, ___, ____) => true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int maxRetries = 0;
            int retryWait = 2;
            if (request.Headers.Contains("MaxRetries"))
            {
                if (!int.TryParse(request.Headers.Get("MaxRetries"), out maxRetries))
                    maxRetries = 0;
                request.Headers.Remove("MaxRetries");
            }

            if (request.Headers.Contains("RetryWait"))
            {
                if (!int.TryParse(request.Headers.Get("RetryWait"), out retryWait))
                    retryWait = 2;
                request.Headers.Remove("RetryWait");
            }

            if (maxRetries == 0)
                return base.SendAsync(request, cancellationToken);

            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryWait, retryAttempt)));

            return retryPolicy.ExecuteAsync(async () =>
            {
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response;
            });
        }
    }

    public static class HttpRequestHeadersExtensions
    {
        public static void Set(this HttpRequestHeaders headers, string name, string value)
        {
            if (headers.Contains(name)) headers.Remove(name);
            headers.Add(name, value);
        }

        public static string Get(this HttpRequestHeaders headers, string name)
        {
            return headers.Contains(name) ? headers.GetValues(name).FirstOrDefault() : null;
        }
    }
}