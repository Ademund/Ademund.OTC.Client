using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ademund.OTC.Client.Model
{
    public class SecurityGroupsCollection
    {
        [JsonProperty("security_groups")]
        public IEnumerable<SecurityGroup> SecurityGroups { get; init; } = new List<SecurityGroup>();
    }
}
