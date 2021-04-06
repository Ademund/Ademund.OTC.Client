using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSConsumerGroupsCollection
    {
        [J("queue_id", NullValueHandling = N.Ignore)] public string QueueId { get; init; }
        [J("queue_name", NullValueHandling = N.Ignore)] public string QueueName { get; init; }
        [J("groups")] public IEnumerable<DMSConsumerGroup> Groups { get; init; } = new List<DMSConsumerGroup>();
        [J("total", NullValueHandling = N.Ignore)] public long? Total { get; init; }
        [J("redrive_policy", NullValueHandling = N.Ignore)] public string RedrivePolicy { get; init; }
    }
}
