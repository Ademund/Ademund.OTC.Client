using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public record DMSMessagesAck
    {
        [J("message")] public IEnumerable<DMSMessageAck> Messages { get; init; }
    }

    public record DMSMessageAck
    {
        [J("handler")] public string Handler { get; init; }
        [J("status")] public DMSMessageAckStatus Status { get; init; }
    }

    public enum DMSMessageAckStatus
    {
        success,
        fail
    }

    public record DMSMessageAckResponse
    {
        [J("success")] public int Success { get; init; }
        [J("fail")] public int Fail { get; init; }
    }
}
