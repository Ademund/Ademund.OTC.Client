using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SecurityGroupRuleResponse
    {
        [J("security_group_rule")] public SecurityGroup SecurityGroupRule { get; init; }
    }
}
