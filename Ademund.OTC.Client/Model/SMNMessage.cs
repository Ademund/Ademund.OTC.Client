using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SMNMessage
    {
        [J("subject")] public string Subject { get; init; }
        [J("message")] public string Message { get; init; }
        [J("time_to_live")] public int TTL { get; init; } = 3600;
    }
}
