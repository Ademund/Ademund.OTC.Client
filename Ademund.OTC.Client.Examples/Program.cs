using Ademund.OTC.Client.Model;
using Ademund.OTC.DMSUtils;
using Ademund.OTC.Examples.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Ademund.OTC.Client.Examples
{
    class Program
    {
        // dotnet user-secrets init
        // dotnet user-secrets set "Examples:AccessKey" "ak"
        // dotnet user-secrets set "Examples:SecretKey" "sk"
        // dotnet user-secrets set "Examples:ProjectId" "pid"

        static async Task Main(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile($"appsettings.{env}.json", optional: true);

            IConfigurationRoot configuration = builder.Build();
            var config = configuration.GetSection("Examples").Get<ExamplesConfig>();

            Console.WriteLine("Config Params: ");
            Console.WriteLine($" - AccessKey: {config.AccessKey}");
            Console.WriteLine($" - ProjectId: {config.ProjectId}");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Choose Example Request: ");
                int choice = 0;
                foreach (var x in config.Examples)
                {
                    Console.WriteLine($" {choice++} - {x.Name}");
                }
                Console.WriteLine(" x - Exit");
                Console.Write("Enter Example Number: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    break;

                var example = config.Examples[choice];
                Console.WriteLine($" - Region: {example.Region}");
                Console.WriteLine($" - Service: {example.Service}");
                Console.WriteLine($" - RequestUri: {example.RequestUri}");
                Console.WriteLine();

                string baseUrl = $"https://dms.{example.Region}.otc.t-systems.com";
                var api = OTCApiClient.InitOTCApi<IOTCDMSApi>(
                    baseUrl: baseUrl,
                    key: config.AccessKey,
                    secret: config.SecretKey,
                    projectId: config.ProjectId,
                    region: example.Region,
                    service: example.Service,
                    config.UseProxy ? config.ProxyAddress : null
                    );

                /*
                var queue = await api.CreateQueue(new DMSQueue() {
                    Name = "magenta-users",
                    Description = "This is a FIFO queue",
                    QueueMode = "FIFO",
                    RedrivePolicy = "enable",
                    MaxConsumeCount = 3
                });
                Console.WriteLine($"queue: {queue}");

                Console.WriteLine("create a consumer group");
                var groupNames = new List<DMSConsumerGroup>() { new DMSConsumerGroup() { Name = "migration" } };
                var groupsCollection = new DMSConsumerGroupsCollection() { Groups = groupNames };
                var createConsomerGroups = await api.CreateConsumerGroups(queue.Id, groupsCollection);
                Console.WriteLine($"createConsomerGroups: {createConsomerGroups}");

                Console.WriteLine("get consumer groups (wait 3secs)");
                Thread.Sleep(3000);

                var getConsumerGroups = await api.GetConsumerGroups(queue.Id);
                Console.WriteLine($"getConsumerGroups: {getConsumerGroups}");

                Console.ReadKey();
                */

                string queueId = "ad2a781a-a675-4e5c-a7e5-8f31d6dfe7a5";
                string consumerId = "g-ad1397f3-9544-4c1c-8cb1-322412a1d41b";
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("send messages");
                    var messages = new List<DMSMessage<TypedMessage>>() {
                        new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = $"TypedMessage1-{i}", Message = "This is a typed message 1", Count = i+1 } },
                        new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = $"TypedMessage2-{i}", Message = "This is a typed message 2", Count = i+2 } },
                        new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = $"TypedMessage3-{i}", Message = "This is a typed message 3", Count = i+3 } },
                        new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = $"TypedMessage4-{i}", Message = "This is a typed message 4", Count = i+4 } },
                        new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = $"TypedMessage5-{i}", Message = "This is a typed message 5", Count = i+5 } },
                    };
                    if (i == 99)
                    {
                        messages.Add(new DMSMessage<TypedMessage>() { Body = new TypedMessage() { Name = "Stop", Message = "This is a Stop message", Count = 0 } });
                    }
                    var messagesCollection = new DMSMessagesCollection<TypedMessage>() { Messages = messages };
                    var createMessages = await api.SendMessages(queueId, messagesCollection);
                    Console.WriteLine($"createMessages: {createMessages}");
                }

                /*
                Console.WriteLine("consume messages");
                var messagePump = new DMSMessagePump(api);
                var subscription = messagePump.Subscribe(queueId, consumerId, 15000);
                subscription.OnMessages += Subscription_OnMessages;
                messagePump.Start();
                Console.WriteLine("Press a key to delete the q");
                Console.ReadKey();
                await api.DeleteQueue(queue.Id);
                */
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    public record TypedMessage
    {
        [J("Name")] public string Name { get; init; }
        [J("Message")] public string Message { get; init; }
        [J("Count")] public int Count { get; init; }
    }
}
