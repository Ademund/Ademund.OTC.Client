using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Ademund.OTC.Client.Model
{
    public sealed record SecurityGroup
    {
        [J("id")] public string Id { get; init; }
        [J("name")] public string Name { get; init; }
        [J("description")] public string Description { get; init; }
        [J("vpc_id", NullValueHandling = N.Ignore)] public string VpcId { get; init; }
        [J("security_group_rules")] public IEnumerable<SecurityGroupRule> Rules { get; init; } = new List<SecurityGroupRule>();
    }
}
