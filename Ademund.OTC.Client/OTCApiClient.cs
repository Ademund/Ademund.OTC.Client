using Ademund.OTC.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestEase;
using System;
using System.Net;
using System.Net.Http;

namespace Ademund.OTC.Client
{
    public class OTCApiClient : IDisposable
    {
        private readonly SigningHttpClientHandler _handler;

        public OTCApiClient(Signer signer, string proxyAddress = null)
        {
            IWebProxy proxy = string.IsNullOrWhiteSpace(proxyAddress) ? null : new WebProxy(proxyAddress);
            _handler = new SigningHttpClientHandler(signer) { Proxy = proxy, UseProxy = proxy != null };
        }

        public void Dispose()
        {
            _handler?.Dispose();
        }

        public T InitOTCApi<T>(string baseUrl)
        {
            var settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto
            };
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            settings.Converters.Add(new VersionConverter());

            var httpClient = new HttpClient(_handler)
            {
                BaseAddress = new Uri(baseUrl)
            };

            return new RestClient(httpClient)
            {
                ResponseDeserializer = new CustomJsonResponseDeserializer(),
                JsonSerializerSettings = settings
            }.For<T>();
        }
    }
}
