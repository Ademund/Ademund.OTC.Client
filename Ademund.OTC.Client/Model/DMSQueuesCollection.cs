using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSQueuesCollection
    {
        [J("queues")] public IEnumerable<DMSQueue> Queues { get; init; } = new List<DMSQueue>();
        [J("total")] public long Total { get; init; }
    }
}
