namespace EventBus
{
    public interface IMessageHandler
    {
        void HandleMessage(string message);
    }
}