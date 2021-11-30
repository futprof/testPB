using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EventBus
{
    public class EventBusRabbitMQ : BackgroundService, IEventBus
    {
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IMessageHandler _messageHandler;
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _queueName;

        public EventBusRabbitMQ(
            ILogger<EventBusRabbitMQ> logger,
            IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IMessageHandler messageHandler)
        {
            _logger = logger;
            _hostName = rabbitMqOptions.Value.Hostname;
            _port = rabbitMqOptions.Value.Port;
            _queueName = rabbitMqOptions.Value.QueueName;
            _messageHandler = messageHandler;
            InitializeConnactionFactory();
        }

        private void InitializeConnactionFactory()
        {            
            _logger.LogInformation("[{Date}] Creating RabbitMQ factory.", DateTime.Now.ToString());
            var factory = new ConnectionFactory() { HostName = _hostName, Port = _port };

            _logger.LogInformation("[{Date}] Creating RabbitMQ channel.", DateTime.Now.ToString());
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        public void Publish(string message)
        {
            _logger.LogInformation("[{Date}] Creating new RabbitMQ channel to publish message.", DateTime.Now.ToString());
            using (var channel = _connection.CreateModel())
            {   
                var body = Encoding.UTF8.GetBytes(message);

                _logger.LogInformation("[{Date}] Publishing message to RabbitMQ.", DateTime.Now.ToString());
                channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                channel.BasicPublish(exchange: "",
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }
        }

        public void Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);

            _logger.LogInformation("[{Date}] Consume messages.", DateTime.Now.ToString());
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                //handle recieved message
                _logger.LogInformation("[{Date}] Handling recieved message.", DateTime.Now.ToString());
                _messageHandler.HandleMessage(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            //TODO: need handler here
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            Consume();
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
