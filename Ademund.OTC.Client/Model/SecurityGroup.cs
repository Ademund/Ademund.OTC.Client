using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ademund.OTC.Client.Model
{
    public record SecurityGroup
    {
        [JsonProperty("id")]
        public string Id { get; init; }
        [JsonProperty("name")]
        public string Name { get; init; }
        [JsonProperty("description")]
        public string Description { get; init; }
        [JsonProperty("vpc_id")]
        public string VpcId { get; init; }
        [JsonProperty("security_group_rules")]
        public IEnumerable<SecurityGroupRule> Rules { get; init; } = new List<SecurityGroupRule>();
    }

    public record SecurityGroupResponse
    {
        [JsonProperty("security_group")]
        public SecurityGroup SecurityGroup { get; init; }
    }
}
