using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record DMSMessagesAck
    {
        [J("message")] public IEnumerable<DMSMessageAck> Messages { get; init; }
    }

    public sealed record DMSMessageAck
    {
        [J("handler")] public string Handler { get; init; }
        [J("status")] public DMSMessageAckStatus Status { get; init; }
    }
}
