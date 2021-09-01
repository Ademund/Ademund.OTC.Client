using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SMNMessageResponse
    {
        [J("message_id")] public string MessageId { get; init; }
        [J("request_id")] public string RequestId { get; init; }
    }
}
