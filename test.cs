using System;
using System.IO;
using Avro;
using Avro.IO;
using Avro.Specific;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Avro schema for Employee
        var schemaJson = @"
        {
          ""type"": ""record"",
          ""name"": ""Employee"",
          ""fields"": [
            { ""name"": ""Id"", ""type"": ""int"" },
            { ""name"": ""Name"", ""type"": ""string"" }
          ]
        }";

        // Parse the schema
        var schema = (RecordSchema)Schema.Parse(schemaJson);

        // Create an instance of Employee
        var employee = new Employee
        {
            Id = 1,
            Name = "Alice"
        };

        // Validate the Employee instance against the schema
        ValidateEmployee(schema, employee);
    }

    static void ValidateEmployee(RecordSchema schema, Employee employee)
    {
        try
        {
            // Serialize the Employee instance
            using (var stream = new MemoryStream())
            {
                var encoder = new BinaryEncoder(stream);
                var writer = new SpecificWriter<Employee>(schema);
                writer.Write(employee, encoder); // Validate here
                Console.WriteLine("Employee data is valid.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}");
        }
    }
}
