using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Child child = new()
            {
                Description = "Description",
                Id = 1,
                Name = "Name",
                Type = "Type"
            };

            Base baseClass = new()
            {
                Description = "Description",
                Id = 1,
                Name = "Name",
                Type = "Type"
            };

            // Serialize objects with formatting
            var data = JsonConvert.SerializeObject(child, Formatting.Indented);
            var data2 = JsonConvert.SerializeObject(baseClass, Formatting.Indented);

            Console.WriteLine(data);
            Console.WriteLine(data2);
        }
    }

    public class Child : Base
    {
        // If you want to override serialization behavior, you can use this
        [JsonProperty("Type")]
        public string Type { get; set; }

        public bool ShouldSerializeType()
        {
            return false; // This will prevent serialization of 'Type' in Child
        }
    }

    public class Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // You can choose whether or not to make 'Type' serialized in the base class
        public string Type { get; set; }
    }
}
