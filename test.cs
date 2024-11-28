{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "type": "object",
  "properties": {
    "id": { "type": "integer" },
    "name": { "type": "string" }
  },
  "required": ["id", "name"]
}


using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

class KafkaProducerWithSchemaRegistry
{
    static async Task Main(string[] args)
    {
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "http://localhost:8081" // URL of your Schema Registry
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092" // Kafka broker
        };

        using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
        using var producer = new ProducerBuilder<string, dynamic>(producerConfig)
            .SetValueSerializer(new JsonSerializer<dynamic>(schemaRegistry))
            .Build();

        var message = new
        {
            id = 1,
            name = "John Doe"
        };

        try
        {
            var result = await producer.ProduceAsync("your-topic-name", new Message<string, dynamic>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });

            Console.WriteLine($"Message sent to partition {result.Partition} with offset {result.Offset}");
        }
        catch (ProduceException<string, dynamic> ex)
        {
            Console.WriteLine($"Message failed: {ex.Message}");
        }
    }
}
