using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace DP.Notifications.Services
{
    public class ServiceBusConsumer
    {
        private readonly QueueClient _queueClient;
        private readonly ILogger _logger;
        public ServiceBusConsumer(IConfiguration configuration, ILogger logger)
        {
            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"), "messages");
            _logger = logger;
        }

        public void Register()
        {
            var option = new MessageHandlerOptions((e) => Task.CompletedTask)
            {
                AutoComplete = false  
            };

            _queueClient.RegisterMessageHandler(ProcessMessage, option);
        }
        private async Task ProcessMessage(Message message, CancellationToken toke)
        {
            var payload = JsonConvert.DeserializeObject<MessagePayload>(Encoding.UTF8.GetString(message.Body));
            
            try
            {
                if (payload.EventName == "NewUserRegistered")
                {
                    EmailSender sender = new EmailSender();
                    sender.SendNewUserEmail(payload.UserEmail, "Test wiadomosci COVID", "Wiadomość testowa o kwarantannie");
                }

                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error busconsumer");
                throw;
            }
            

        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
    public class MessagePayload
    {
        public string EventName { get; set; }

        public string UserEmail { get; set; }
    }
}
