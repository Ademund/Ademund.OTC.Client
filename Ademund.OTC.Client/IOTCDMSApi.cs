using Ademund.OTC.Client.Model;
using RestEase;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCDMSApi : IOTCApiBase
    {
        [Post("/v1.0/{project_id}/queues")]
        Task<DMSQueue> CreateQueueAsync(
            [Body] DMSQueue queue);

        [Get("/v1.0/{project_id}/queues")]
        Task<DMSQueuesCollection> GetQueuesAsync(
            [Query("include_deadletter")] bool includeDeadLetter = false);

        [Get("/v1.0/{project_id}/queues/{queue_id}")]
        Task<DMSQueue> GetQueueAsync(
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadLetter = false);

        [Delete("/v1.0/{project_id}/queues/{queue_id}")]
        Task DeleteQueueAsync(
            [Path("queue_id")] string queueId);

        [Post("/v1.0/{project_id}/queues/{queue_id}/groups")]
        Task<DMSConsumerGroupsCollection> CreateConsumerGroupsAsync(
            [Path("queue_id")] string queueId,
            [Body] DMSConsumerGroupsCollection groups);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups")]
        Task<DMSConsumerGroupsCollection> GetConsumerGroupsAsync(
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadletter = false,
            [Query("page_size")] int pageSize = 100,
            [Query("current_page")] int currentPage = 1
            );

        [Delete("/v1.0/{project_id}/queues/{queue_id}/groups/{group_id}")]
        Task DeleteConsumerGroupAsync(
            [Path("queue_id")] string queueId,
            [Path("group_id")] string groupId);

        [Header("MaxRetries", "3")]
        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        Task<DMSSendMessageResponseCollection> SendMessagesAsync(
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection messages);

        [Header("MaxRetries", "3")]
        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        Task<DMSSendMessageResponseCollection> SendMessagesAsync<T>(
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection<T> messages);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        Task<IEnumerable<DMSConsumeMessageResponse>> ConsumeMessagesAsync(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30,
            CancellationToken cancellationToken = default);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        Task<IEnumerable<DMSConsumeMessageResponse<T>>> ConsumeMessagesAsync<T>(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30,
            CancellationToken cancellationToken = default);

        [Header("MaxRetries", "3")]
        [Post("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/ack")]
        Task<DMSMessageAckResponse> AckMessagesAsync(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Body] DMSMessagesAck messages,
            CancellationToken cancellationToken = default);
    }
}
