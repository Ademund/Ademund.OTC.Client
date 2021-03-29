using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public record DMSMessage
    {
        [J("body")] public object Body { get; init; }
        [J("attributes", NullValueHandling = N.Ignore)] public IReadOnlyDictionary<string, string> Attributes { get; init; }
    }

    public record DMSMessage<T>
    {
        [J("body")] public T Body { get; init; }
        [J("attributes", NullValueHandling = N.Ignore)] public IReadOnlyDictionary<string, string> Attributes { get; init; }
    }
}
