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
    public static class OTCApiClient
    {
        public static T InitOTCApi<T>(string baseUrl, string key, string secret, string projectId, string region = null, string service = null, string proxyAddress = null) where T: IOTCApiBase
        {
            IWebProxy proxy = string.IsNullOrWhiteSpace(proxyAddress) ? null : new WebProxy(proxyAddress);
            var signer = new Signer(key, secret, region, service);
            var handler = new SigningHttpClientHandler(signer) { Proxy = proxy, UseProxy = proxy != null };
            var httpClient = new HttpClient(handler) {
                BaseAddress = new Uri(baseUrl)
            };

            var settings = new JsonSerializerSettings() {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto
            };
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            settings.Converters.Add(new VersionConverter());

            var restClient = new RestClient(httpClient) {
                ResponseDeserializer = new CustomJsonResponseDeserializer(),
                JsonSerializerSettings = settings
            };

            var api = restClient.For<T>();
            api.XProjectId = projectId;
            api.ProjectId = projectId;

            return api;
        }
    }
}
