using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SecurityGroupsCollection
    {
        [J("security_groups")] public IEnumerable<SecurityGroup> SecurityGroups { get; init; } = new List<SecurityGroup>();
    }
}
