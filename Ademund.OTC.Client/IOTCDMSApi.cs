using Ademund.OTC.Client.Model;
using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCDMSApi : IOTCApiBase
    {
        [Post("/v1.0/{project_id}/queues")]
        Task<DMSQueue> CreateQueue(
            [Body] DMSQueue queue);

        [Get("/v1.0/{project_id}/queues")]
        Task<DMSQueuesCollection> GetQueues(
            [Query("include_deadletter")] bool includeDeadLetter = false);

        [Get("/v1.0/{project_id}/queues/{queue_id}")]
        Task<DMSQueue> GetQueue(
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadLetter = false);

        [Delete("/v1.0/{project_id}/queues/{queue_id}")]
        Task DeleteQueue(
            [Path("queue_id")] string queueId);

        [Post("/v1.0/{project_id}/queues/{queue_id}/groups")]
        Task<DMSConsumerGroupsCollection> CreateConsumerGroups(
            [Path("queue_id")] string queueId,
            [Body] DMSConsumerGroupsCollection groups);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups")]
        Task<DMSConsumerGroupsCollection> GetConsumerGroups(
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadletter = false,
            [Query("page_size")] int pageSize = 100,
            [Query("current_page")] int currentPage = 1
            );

        [Delete("/v1.0/{project_id}/queues/{queue_id}/groups/{group_id}")]
        Task DeleteConsumerGroup(
            [Path("queue_id")] string queueId,
            [Path("group_id")] string groupId);

        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        Task<DMSSendMessageResponseCollection> SendMessages(
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection messages);

        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        Task<DMSSendMessageResponseCollection> SendMessages<T>(
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection<T> messages);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        Task<IEnumerable<DMSConsumeMessageResponse>> ConsumeMessages(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        Task<IEnumerable<DMSConsumeMessageResponse<T>>> ConsumeMessages<T>(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30);

        [Header("MaxRetries", "3")]
        [Post("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/ack")]
        Task<DMSMessageAckResponse> AckMessages(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Body] DMSMessagesAck messages
            );
    }
}
