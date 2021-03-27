using Ademund.OTC.Client.Model;
using RestEase;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCVPCApi : IOTCApiBase
    {
        // https:///v1/{project_id}/security-groups?limit=3
        [Get("/v1/{project_id}/security-groups")]
        Task<SecurityGroupsCollection> GetSecurityGroups(
            [Path("project_id")] string projectId,
            [Query("vpc_id")] string vpcId = null,
            [Query("marker")] string marker = null,
            [Query("limit")] int limit = 100);

        [Get("/v1/{project_id}/security-groups/{security_group_id}")]
        Task<SecurityGroupResponse> GetSecurityGroup(
            [Path("project_id")] string projectId,
            [Path("security_group_id")] string securityGroupId);

        [Post("/v1/{project_id}/security-group-rules")]
        Task<SecurityGroupRuleResponse> CreateSecurityGroupRule(
            [Path("project_id")] string projectId,
            [Body] SecurityGroupRuleRequest securityGroupRule);

        [Delete("/v1/{project_id}/security-group-rules/{security_group_rule_id}")]
        Task DeleteSecurityGroupRule(
            [Path("project_id")] string projectId,
            [Path("security_group_rule_id")] string securityGroupRuleId);

        [Put("/v1/{project_id}/security-group-rules/{security_group_rule_id}")]
        Task UpdateSecurityGroupRule(
            [Path("project_id")] string projectId,
            [Path("security_group_rule_id")] string securityGroupRuleId,
            [Body] SecurityGroupRuleRequest securityGroupRule);
    }
}
