{
  "type": "record",
  "name": "User",
  "namespace": "example",
  "fields": [
    { "name": "id", "type": "int" },
    { "name": "name", "type": "string" },
    { "name": "email", "type": "string" },
    { "name": "createdAt", "type": { "type": "long", "logicalType": "timestamp-millis" } }
  ]
}


using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using example; // Namespace of the generated Avro class

namespace KafkaAvroProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Schema Registry configuration
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "http://localhost:8081" // Replace with your Schema Registry URL
            };

            // Kafka producer configuration
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092", // Replace with your Kafka broker(s)
                Acks = Acks.All // Ensure the producer waits for acknowledgment
            };

            var topic = "user-topic";

            // Create Schema Registry client and producer
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producer = new ProducerBuilder<string, User>(producerConfig)
                .SetValueSerializer(new AvroSerializer<User>(schemaRegistry))
                .Build();

            // Create a User object
            var user = new User
            {
                id = 1,
                name = "John Doe",
                email = "johndoe@example.com",
                createdAt = DateTime.UtcNow // Ensure UTC
            };

            Console.WriteLine("Sending message to Kafka...");

            try
            {
                // Produce the message
                var deliveryResult = await producer.ProduceAsync(topic, new Message<string, User>
                {
                    Key = "user-1", // Key for partitioning
                    Value = user
                });

                Console.WriteLine($"Message delivered to {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<string, User> ex)
            {
                Console.WriteLine($"Error producing message: {ex.Error.Reason}");
            }
        }
    }
}


using Avro;
using Avro.Specific;

namespace example
{
    public class User : ISpecificRecord
    {
        public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"User\",\"namespace\":\"example\",\"fields\":[{\"name\":\"id\",\"type\":\"int\"},{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"email\",\"type\":\"string\"},{\"name\":\"createdAt\",\"type\":{\"type\":\"long\",\"logicalType\":\"timestamp-millis\"}}]}");

        public virtual Schema Schema => _SCHEMA;

        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
    }
}


dotnet add package Confluent.Kafka
dotnet add package Confluent.SchemaRegistry
dotnet add package Confluent.SchemaRegistry.Serdes
avrogen -s user.avsc .
