using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SMNTopic
    {
        [J("topic_urn")] public string TopicUrn { get; init; }
        [J("display_name")] public string DisplayName { get; init; }
        [J("name")] public string Name { get; init; }
        [J("push_policy")] public int? PushPolicy { get; init; }
    }
}
