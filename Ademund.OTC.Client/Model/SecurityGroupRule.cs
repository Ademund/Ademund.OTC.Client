using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Model
{
    public sealed record SecurityGroupRule
    {
        [J("id")] public string Id { get; init; }
        [J("description")] public string Description { get; init; }
        [J("direction")] public string Direction { get; init; }
        [J("ethertype")] public string EtherType { get; init; }
        [J("protocol")] public string Protocol { get; init; }
        [J("port_range_min")] public int? PortRangeMin { get; init; }
        [J("port_range_max")] public int? PortRangeMax { get; init; }
        [J("remote_ip_prefix")] public string RemoteIpPrefix { get; init; }
        [J("remote_group_id")] public string RemoteGroupId { get; init; }
        [J("security_group_id")] public string SecurityGroupId { get; init; }
        [J("tenant_id")] public string TenantId { get; init; }
    }
}
