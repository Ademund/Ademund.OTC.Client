using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public record SMNTopicsCollection
    {
        [J("request_id")] public string RequestId { get; init; }
        [J("topic_count")] public int TopicCount { get; init; } = 0;
        [J("topics")] public IEnumerable<SMNTopic> Topics { get; init; } = new List<SMNTopic>();
    }
}
