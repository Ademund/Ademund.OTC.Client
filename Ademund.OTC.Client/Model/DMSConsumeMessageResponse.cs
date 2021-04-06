using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSConsumeMessageResponse
    {
        [J("handler", NullValueHandling = N.Ignore)] public string Handler { get; init; }
        [J("message")] public DMSMessage Message { get; init; }
    }

    public sealed record DMSConsumeMessageResponse<T>
    {
        [J("handler", NullValueHandling = N.Ignore)] public string Handler { get; init; }
        [J("message")] public DMSMessage<T> Message { get; init; }
    }
}
