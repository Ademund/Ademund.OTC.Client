using Newtonsoft.Json;

namespace Ademund.OTC.Client.Model
{
    public record SMNTopic
    {
        [JsonProperty("topic_urn")]
        public string TopicUrn { get; init; }
        [JsonProperty("display_name")]
        public string DisplayName { get; init; }
        [JsonProperty("name")]
        public string Name { get; init; }
        [JsonProperty("push_policy")]
        public int? PushPolicy { get; init; }
    }
}
