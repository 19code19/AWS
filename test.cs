using System;
using System.IO;
using Avro;
using Avro.IO;
using Avro.Specific;
using Avro.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Define Avro schema as JSON string
        var schemaJson = @"
        {
          ""type"": ""record"",
          ""name"": ""User"",
          ""fields"": [
            { ""name"": ""id"", ""type"": ""int"" },
            { ""name"": ""name"", ""type"": ""string"" }
          ]
        }";

        // Parse the schema
        var schema = Schema.Parse(schemaJson);

        // Create valid and invalid data
        var validData = new GenericRecord((RecordSchema)schema)
        {
            { "id", 1 },
            { "name", "Alice" }
        };

        var invalidData = new GenericRecord((RecordSchema)schema)
        {
            { "id", "invalid" }, // Invalid type
            { "name", 123 }      // Invalid type
        };

        // Validate valid data
        ValidateData(schema, validData);

        // Validate invalid data
        ValidateData(schema, invalidData);
    }

    static void ValidateData(Schema schema, GenericRecord data)
    {
        try
        {
            // Serialize the data to validate
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryEncoder(stream);
                var datumWriter = new GenericWriter<GenericRecord>(schema);
                datumWriter.Write(data, writer);
                Console.WriteLine("Data is valid.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}");
        }
    }
}
