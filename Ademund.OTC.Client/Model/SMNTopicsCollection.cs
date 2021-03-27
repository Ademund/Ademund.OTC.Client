using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ademund.OTC.Client.Model
{
    public record SMNTopicsCollection
    {
        [JsonProperty("request_id")]
        public string RequestId { get; init; }
        [JsonProperty("topic_count")]
        public int TopicCount { get; init; } = 0;
        [JsonProperty("topics")]
        public IEnumerable<SMNTopic> Topics { get; init; } = new List<SMNTopic>();
    }
}
