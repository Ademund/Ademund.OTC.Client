using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public record DMSConsumerGroup
    {
        [J("id", NullValueHandling = N.Ignore)] public string Id { get; set; }
        [J("name")] public string Name { get; set; }
        [J("consumed_messages", NullValueHandling = N.Ignore)] public long? ConsumedMessages { get; set; }
        [J("available_messages", NullValueHandling = N.Ignore)] public long? AvailableMessages { get; set; }
        [J("produced_messages", NullValueHandling = N.Ignore)] public long? ProducedMessages { get; set; }
        [J("available_deadletters", NullValueHandling = N.Ignore)] public long? AvailableDeadletters { get; set; }
        [J("produced_deadletters", NullValueHandling = N.Ignore)] public long? ProducedDeadletters { get; set; }
    }
}
