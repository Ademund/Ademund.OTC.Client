using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public record DMSQueue
    {
        [J("id", NullValueHandling = N.Ignore)] public string Id { get; init; }
        [J("name")] public string Name { get; init; }
        [J("created", NullValueHandling = N.Ignore)] public string Created { get; init; }
        [J("description")] public string Description { get; init; }
        [J("queue_mode")] public string QueueMode { get; init; }
        [J("reservation", NullValueHandling = N.Ignore)] public int? Reservation { get; init; }
        [J("max_msg_size_byte", NullValueHandling = N.Ignore)] public int? MaxMsgSizeByte { get; init; }
        [J("produced_messages", NullValueHandling = N.Ignore)] public int? ProducedMessages { get; init; }
        [J("redrive_policy", NullValueHandling = N.Ignore)] public string RedrivePolicy { get; init; }
        [J("max_consume_count", NullValueHandling = N.Ignore)] public int? MaxConsumeCount { get; init; }
        [J("group_count", NullValueHandling = N.Ignore)] public int? GroupCount { get; init; }
        [J("eff_date", NullValueHandling = N.Ignore)] public string EffDate { get; init; }
    }
}
