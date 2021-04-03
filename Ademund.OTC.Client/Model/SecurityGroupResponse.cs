using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SecurityGroupResponse
    {
        [J("security_group")] public SecurityGroup SecurityGroup { get; init; }
    }
}
