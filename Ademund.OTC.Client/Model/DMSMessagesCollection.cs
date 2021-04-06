using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSMessagesCollection
    {
        [J("messages")] public IEnumerable<DMSMessage> Messages { get; init; } = new List<DMSMessage>();
    }

    public sealed record DMSMessagesCollection<T>
    {
        [J("messages")] public IEnumerable<DMSMessage<T>> Messages { get; init; } = new List<DMSMessage<T>>();
    }
}
