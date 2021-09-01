using Ademund.OTC.Client.Model;
using RestEase;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCSMNApi : IOTCApiBase
    {
        [Get("/v2/{project_id}/notifications/topics")]
        Task<SMNTopicsCollection> GetTopics([Path("project_id")] string projectId, [Query("limit")] int limit = 100, [Query("offset")] int offset = 0);

        [Post("/v2/{project_id}/notifications/topics/{topic_urn}/publish")]
        Task<SMNMessageResponse> PublishMessage([Path("project_id")] string projectId, [Path("topic_urn", UrlEncode = false)] string topicUrn, [Body] SMNMessage message);
    }
}
