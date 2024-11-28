using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

class Program
{
    static async Task Main(string[] args)
    {
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "http://localhost:8081" // Schema Registry URL
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092" // Kafka Broker
        };

        using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
        using var producer = new ProducerBuilder<string, User>(producerConfig)
            .SetValueSerializer(new AvroSerializer<User>(schemaRegistry))
            .Build();

        var user = new User { id = 1, name = "John Doe", email = "john.doe@example.com" };

        var result = await producer.ProduceAsync("user-topic", new Message<string, User>
        {
            Key = "user1",
            Value = user
        });

        Console.WriteLine($"Produced message to: {result.TopicPartitionOffset}");
    }
}


dotnet add package Confluent.Kafka
dotnet add package Confluent.SchemaRegistry
dotnet add package Confluent.SchemaRegistry.Serdes
dotnet add package Avro
