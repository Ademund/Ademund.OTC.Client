using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public record DMSQueuesCollection
    {
        [J("queues")] public IEnumerable<DMSQueue> Queues { get; init; }
        [J("total")] public long Total { get; init; }
    }
}
