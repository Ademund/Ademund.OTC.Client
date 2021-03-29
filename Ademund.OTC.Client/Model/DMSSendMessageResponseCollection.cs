using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public record DMSSendMessageResponseCollection
    {
        [J("messages")] public IEnumerable<DMSSendMessageResponse> Messages { get; init; } = new List<DMSSendMessageResponse>();
    }
}
