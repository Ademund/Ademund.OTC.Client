using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public record DMSConsumerGroup
    {
        [J("id", NullValueHandling = N.Ignore)] public string Id { get; init; }
        [J("name")] public string Name { get; init; }
        [J("consumed_messages", NullValueHandling = N.Ignore)] public long? ConsumedMessages { get; init; }
        [J("available_messages", NullValueHandling = N.Ignore)] public long? AvailableMessages { get; init; }
        [J("produced_messages", NullValueHandling = N.Ignore)] public long? ProducedMessages { get; init; }
        [J("available_deadletters", NullValueHandling = N.Ignore)] public long? AvailableDeadletters { get; init; }
        [J("produced_deadletters", NullValueHandling = N.Ignore)] public long? ProducedDeadletters { get; init; }
    }
}
