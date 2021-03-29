using Ademund.OTC.Client.Model;
using Ademund.OTC.Examples.Config;
using Ademund.OTC.Utils;
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

                var signer = new Signer(config.AccessKey, config.SecretKey, example.Region, example.Service);
                using var client = new OTCApiClient(signer, config.UseProxy ? config.ProxyAddress : null);
                var api = client.InitOTCApi<IOTCDMSApi>($"https://dms.{example.Region}.otc.t-systems.com");
                api.ProjectId = config.ProjectId;

                var queue = await api.CreateQueue(config.ProjectId, new DMSQueue() {
                    Name = "test-queue",
                    Description = "This is a FIFO queue",
                    QueueMode = "FIFO",
                    RedrivePolicy = "enable",
                    MaxConsumeCount = 3
                });
                Console.WriteLine($"queue: {queue}");

                Console.WriteLine("create a consumer group");
                var groupNames = new List<DMSConsumerGroup>() { new DMSConsumerGroup() { Name = "Test-Consumer-Group" } };
                var groupsCollection = new DMSConsumerGroupsCollection() { Groups = groupNames };
                var createConsomerGroups = await api.CreateConsumerGroups(config.ProjectId, queue.Id, groupsCollection);
                Console.WriteLine($"createConsomerGroups: {createConsomerGroups}");

                Console.WriteLine("get consumer groups (wait 3secs)");
                Thread.Sleep(3000);

                var getConsumerGroups = await api.GetConsumerGroups(config.ProjectId, queue.Id);
                Console.WriteLine($"getConsumerGroups: {getConsumerGroups}");

                Console.WriteLine("send a message");
                var messages = new List<DMSMessage<TypedMessage>>() {
                    new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = "TypedMessage1", Message = "This is a typed message 1", Count = 1 } },
                    new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = "TypedMessage2", Message = "This is a typed message 2", Count = 2 } },
                    new DMSMessage<TypedMessage>() {Body = new TypedMessage() {Name = "TypedMessage3", Message = "This is a typed message 3", Count = 3 } },
                };
                var messagesCollection = new DMSMessagesCollection<TypedMessage>() { Messages = messages };
                var createMessages = await api.SendMessages(config.ProjectId, queue.Id, messagesCollection);
                Console.WriteLine($"createMessages: {createMessages}");

                Console.WriteLine("consume messages");
                var comsumeMessages = await api.ConsumeMessages<TypedMessage>(config.ProjectId, queue.Id, createConsomerGroups.Groups.First().Id);
                Console.WriteLine($"comsumeMessages: {comsumeMessages}");
                foreach(var message in comsumeMessages)
                {
                    Console.WriteLine($" - message: {message}");
                    Console.WriteLine($" --> Name: {message.Message.Body?.Name}");
                    Console.WriteLine($" --> Message: {message.Message.Body?.Message}");
                    Console.WriteLine($" --> Count: {message.Message.Body?.Count}");
                }

                Console.WriteLine("Press a key to delete the q");
                Console.ReadKey();
                await api.DeleteQueue(config.ProjectId, queue.Id);

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
