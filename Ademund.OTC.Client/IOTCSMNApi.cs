using Ademund.OTC.Client.Model;
using RestEase;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCSMNApi : IOTCApiBase
    {
        [Get("/v2/{project_id}/notifications/topics")]
        Task<SMNTopicsCollection> GetTopics([Path("project_id")] string projectId, [Query("limit")] int limit = 100, [Query("offset")] int offset = 0);
    }
}
