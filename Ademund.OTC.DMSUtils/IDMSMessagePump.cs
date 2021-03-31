namespace Ademund.OTC.DMSUtils
{
    public interface IDMSMessagePump
    {
        void Start();
        void Stop();
        void Pause();
        void Wait();
        IDMSMessagePumpSubscription Subscribe(string queueId, string consumerGroupId, int pollInterval);
        void UnSubscribe(string queueId, string consumerGroupId);
    }
}
