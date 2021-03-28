using Ademund.OTC.Client.Model;
using RestEase;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCDMSApi : IOTCApiBase
    {
        [Post("/v1.0/{project_id}/queues")]
        Task<DMSQueue> CreateQueue(
            [Path("project_id")] string projectId,
            [Body] DMSQueue queue);

        [Get("/v1.0/{project_id}/queues")]
        Task<DMSQueuesCollection> GetQueues(
            [Path("project_id")] string projectId,
            [Query("include_deadletter")] bool includeDeadLetter = false);

        [Get("/v1.0/{project_id}/queues/{queue_id}")]
        Task<DMSQueue> GetQueue(
            [Path("project_id")] string projectId,
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadLetter = false);

        [Delete("/v1.0/{project_id}/queues/{queue_id}")]
        Task DeleteQueue(
            [Path("project_id")] string projectId,
            [Path("queue_id")] string queueId);
    }
}
