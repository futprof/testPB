namespace EventBus
{
    public interface IEventBus
    {
        //publish message to queue
        void Publish(string message);
        //consume message from queue
        void Consume();
    }
}