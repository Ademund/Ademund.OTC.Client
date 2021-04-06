using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSSendMessageResponse
    {
        [J("error", NullValueHandling = N.Ignore)] public string Error { get; init; }
        [J("error_code", NullValueHandling = N.Ignore)] public int ErrorCode { get; init; }
        [J("state")] public DMSMessageState State { get; init; }
    }
}
