using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSMessageAckResponse
    {
        [J("success")] public int Success { get; init; }
        [J("fail")] public int Fail { get; init; }
    }
}
