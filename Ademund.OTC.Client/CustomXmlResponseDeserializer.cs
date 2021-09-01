using RestEase;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;

namespace Ademund.OTC.Client
{
    public class CustomXmlResponseDeserializer : ResponseDeserializer
    {
        private readonly static ConcurrentDictionary<Type, XmlSerializer> _serializers = new();

        public override T Deserialize<T>(string content, HttpResponseMessage response, ResponseDeserializerInfo info)
        {
            var serializer = _serializers.GetOrAdd(typeof(T), new XmlSerializer(typeof(T)));
            using (var stringReader = new StringReader(content))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
