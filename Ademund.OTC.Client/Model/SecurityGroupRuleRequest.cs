using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SecurityGroupRuleRequest
    {
        [J("security_group_rule")] public SecurityGroupRule SecurityGroupRule { get; init; }
    }
}
