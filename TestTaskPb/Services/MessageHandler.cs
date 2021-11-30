using EventBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskPb.Models;

namespace TestTaskPb.Services
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public MessageHandler(
            ILogger<MessageHandler> logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public void HandleMessage(string message)
        {
            message = message.Replace(@"_", "");
            var request = JsonConvert.DeserializeObject<FullRequest>(message);
            
            var msg = _repository.SaveCashRequest(request);
            _logger.LogInformation("[{Date}] {msg}", DateTime.Now.ToString(), msg);
        }
    }
}
