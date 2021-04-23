using Ademund.OTC.Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public static class IOTCDMSApiSyncExtensions
    {
        public static DMSQueue CreateQueue(this IOTCDMSApi api,
            DMSQueue queue)
        {
            return Task.Run(() => api.CreateQueueAsync(queue))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSQueuesCollection GetQueues(this IOTCDMSApi api,
            bool includeDeadLetter = false)
        {
            return Task.Run(() => api.GetQueuesAsync(includeDeadLetter))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSQueue GetQueue(this IOTCDMSApi api,
            string queueId, bool includeDeadLetter = false)
        {
            return Task.Run(() => api.GetQueueAsync(queueId, includeDeadLetter))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static void DeleteQueue(this IOTCDMSApi api,
            string queueId)
        {
            Task.Run(() => api.DeleteQueueAsync(queueId))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSConsumerGroupsCollection CreateConsumerGroups(this IOTCDMSApi api,
            string queueId, DMSConsumerGroupsCollection groups)
        {
            return Task.Run(() => api.CreateConsumerGroupsAsync(queueId, groups))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSConsumerGroupsCollection GetConsumerGroups(this IOTCDMSApi api,
            string queueId, bool includeDeadletter = false, int pageSize = 100, int currentPage = 1)
        {
            return Task.Run(() => api.GetConsumerGroupsAsync(queueId, includeDeadletter, pageSize, currentPage))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static void DeleteConsumerGroup(this IOTCDMSApi api,
            string queueId, string groupId)
        {
            Task.Run(() => api.DeleteConsumerGroupAsync(queueId, groupId))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSSendMessageResponseCollection SendMessages(this IOTCDMSApi api,
            string queueId, DMSMessagesCollection messages)
        {
            return Task.Run(() => api.SendMessagesAsync(queueId, messages))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSSendMessageResponseCollection SendMessages<T>(this IOTCDMSApi api,
            string queueId, DMSMessagesCollection<T> messages)
        {
            return Task.Run(() => api.SendMessagesAsync(queueId, messages))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static IEnumerable<DMSConsumeMessageResponse> ConsumeMessages(this IOTCDMSApi api,
            string queueId, string groupId, int maxMessages = 10, int timeWait = 3, int ackWait = 30)
        {
            return Task.Run(() => api.ConsumeMessagesAsync(queueId, groupId, maxMessages, timeWait, ackWait))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static IEnumerable<DMSConsumeMessageResponse<T>> ConsumeMessages<T>(this IOTCDMSApi api,
            string queueId, string groupdId, int maxMessages = 10, int timeWait = 3, int ackWait = 30)
        {
            return Task.Run(() => api.ConsumeMessagesAsync<T>(queueId, groupdId, maxMessages, timeWait, ackWait))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static DMSMessageAckResponse AckMessages(this IOTCDMSApi api,
            string queueId, string groupdId, DMSMessagesAck messages)
        {
            return Task.Run(() => api.AckMessagesAsync(queueId, groupdId, messages))
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
