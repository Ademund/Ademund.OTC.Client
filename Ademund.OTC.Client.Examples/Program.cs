using Ademund.OTC.Client.Model;
using Ademund.OTC.Examples.Config;
using Ademund.OTC.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

                Console.WriteLine("Press a key to create a consumer group");
                Console.ReadKey();

                var groupNames = new List<DMSConsumerGroup>() { new DMSConsumerGroup() { Name = "Test-Consumer-Group" } };
                var groupsCollection = new DMSConsumerGroupsCollection() { Groups = groupNames };
                var createConsomerGroups = await api.CreateConsumerGroups(config.ProjectId, queue.Id, groupsCollection);
                Console.WriteLine($"createConsomerGroups: {createConsomerGroups}");

                Console.WriteLine("Press a key to get consumer groups (wait 3secs)");
                Console.ReadKey();

                var getConsumerGroups = await api.GetConsumerGroups(config.ProjectId, queue.Id);
                Console.WriteLine($"getConsumerGroups: {getConsumerGroups}");

                Console.WriteLine("Press a key to delete a consumer group");
                Console.ReadKey();
                await api.DeleteConsumerGroup(config.ProjectId, queue.Id, getConsumerGroups.Groups.First().Id);

                Console.WriteLine("Press a key to get consumer groups (wait 3secs)");
                Console.ReadKey();
                getConsumerGroups = await api.GetConsumerGroups(config.ProjectId, queue.Id);
                Console.WriteLine($"getConsumerGroups: {getConsumerGroups}");

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
