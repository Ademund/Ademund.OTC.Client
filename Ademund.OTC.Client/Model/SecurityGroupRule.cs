using Newtonsoft.Json;

namespace Ademund.OTC.Client.Model
{
    public record SecurityGroupRule
    {
        [JsonProperty("id")]
        public string Id { get; init; }
        [JsonProperty("description")]
        public string Description { get; init; }
        [JsonProperty("direction")]
        public string Direction { get; init; }
        [JsonProperty("ethertype")]
        public string EtherType { get; init; }
        [JsonProperty("protocol")]
        public string Protocol { get; init; }
        [JsonProperty("port_range_min")]
        public int? PortRangeMin { get; init; }
        [JsonProperty("port_range_max")]
        public int? PortRangeMax { get; init; }
        [JsonProperty("remote_ip_prefix")]
        public string RemoteIpPrefix { get; init; }
        [JsonProperty("remote_group_id")]
        public string RemoteGroupId { get; init; }
        [JsonProperty("security_group_id")]
        public string SecurityGroupId { get; init; }
        [JsonProperty("tenant_id")]
        public string TenantId { get; init; }
    }

    public record SecurityGroupRuleRequest
    {
        [JsonProperty("security_group_rule")]
        public SecurityGroupRule SecurityGroupRule { get; init; }
    }

    public record SecurityGroupRuleResponse
    {
        [JsonProperty("security_group_rule")]
        public SecurityGroup SecurityGroupRule { get; init; }
    }
}
