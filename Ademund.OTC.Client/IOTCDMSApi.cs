using Ademund.OTC.Client.Model;
using RestEase;
using System.Collections.Generic;
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

        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        Task<DMSSendMessageResponseCollection> SendMessagesAsync(
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection messages);

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
            [Query("ack_wait")] int ackWait = 30);

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        Task<IEnumerable<DMSConsumeMessageResponse<T>>> ConsumeMessagesAsync<T>(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30);

        [Header("MaxRetries", "3")]
        [Post("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/ack")]
        Task<DMSMessageAckResponse> AckMessagesAsync(
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Body] DMSMessagesAck messages
            );
    }

    public static class IOTCDMSApiExtensions
    {
        [Post("/v1.0/{project_id}/queues")]
        public static DMSQueue CreateQueue(this IOTCDMSApi api,
            [Body] DMSQueue queue)
        {
            return Task.Run(() => api.CreateQueueAsync(queue))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Get("/v1.0/{project_id}/queues")]
        public static DMSQueuesCollection GetQueues(this IOTCDMSApi api,
            [Query("include_deadletter")] bool includeDeadLetter = false)
        {
            return Task.Run(() => api.GetQueuesAsync(includeDeadLetter))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Get("/v1.0/{project_id}/queues/{queue_id}")]
        public static DMSQueue GetQueue(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadLetter = false)
        {
            return Task.Run(() => api.GetQueueAsync(queueId, includeDeadLetter))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Delete("/v1.0/{project_id}/queues/{queue_id}")]
        public static void DeleteQueue(this IOTCDMSApi api,
            [Path("queue_id")] string queueId)
        {
            Task.Run(() => api.DeleteQueueAsync(queueId))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Post("/v1.0/{project_id}/queues/{queue_id}/groups")]
        public static DMSConsumerGroupsCollection CreateConsumerGroups(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Body] DMSConsumerGroupsCollection groups)
        {
            return Task.Run(() => api.CreateConsumerGroupsAsync(queueId, groups))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups")]
        public static DMSConsumerGroupsCollection GetConsumerGroups(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Query("include_deadletter")] bool includeDeadletter = false,
            [Query("page_size")] int pageSize = 100,
            [Query("current_page")] int currentPage = 1
            )
        {
            return Task.Run(() => api.GetConsumerGroupsAsync(queueId, includeDeadletter, pageSize, currentPage))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Delete("/v1.0/{project_id}/queues/{queue_id}/groups/{group_id}")]
        public static void DeleteConsumerGroup(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Path("group_id")] string groupId)
        {
            Task.Run(() => api.DeleteConsumerGroupAsync(queueId, groupId))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        public static DMSSendMessageResponseCollection SendMessages(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection messages)
        {
            return Task.Run(() => api.SendMessagesAsync(queueId, messages))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Post("/v1.0/{project_id}/queues/{queue_id}/messages")]
        public static DMSSendMessageResponseCollection SendMessages<T>(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Body] DMSMessagesCollection<T> messages)
        {
            return Task.Run(() => api.SendMessagesAsync(queueId, messages))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        public static IEnumerable<DMSConsumeMessageResponse> ConsumeMessages(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30)
        {
            return Task.Run(() => api.ConsumeMessagesAsync(queueId, groupId, maxMessages, timeWait, ackWait))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Get("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/messages")]
        public static IEnumerable<DMSConsumeMessageResponse<T>> ConsumeMessages<T>(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Query("max_msgs")] int maxMessages = 10,
            [Query("time_wait")] int timeWait = 3,
            [Query("ack_wait")] int ackWait = 30)
        {
            return Task.Run(() => api.ConsumeMessagesAsync<T>(queueId, groupdId, maxMessages, timeWait, ackWait))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Header("MaxRetries", "3")]
        [Post("/v1.0/{project_id}/queues/{queue_id}/groups/{consumer_group_id}/ack")]
        public static DMSMessageAckResponse AckMessages(this IOTCDMSApi api,
            [Path("queue_id")] string queueId,
            [Path("consumer_group_id")] string groupdId,
            [Body] DMSMessagesAck messages
            )
        {
            return Task.Run(() => api.AckMessagesAsync(queueId, groupdId, messages))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
