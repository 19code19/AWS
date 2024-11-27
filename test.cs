using Confluent.Kafka;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

//dotnet add package Confluent.Kafka

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "your-kafka-broker:9092",
            SaslMechanism = SaslMechanism.OAuthBearer,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslOauthbearerTokenProvider = async () => await GetOAuthTokenAsync()
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        Console.WriteLine("Enter a message to send to Kafka (type 'exit' to quit):");
        while (true)
        {
            var message = Console.ReadLine();
            if (message?.ToLower() == "exit") break;

            try
            {
                var deliveryResult = await producer.ProduceAsync("your-topic-name", new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                });

                Console.WriteLine($"Delivered to: {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Error: {ex.Error.Reason}");
            }
        }
    }

    private static async Task<string> GetOAuthTokenAsync()
    {
        // Replace with your OAuth token provider details
        var clientId = "your-client-id";
        var clientSecret = "your-client-secret";
        var tokenEndpoint = "https://your-oauth-provider.com/token";

        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
            })
        };

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch OAuth token. Status code: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(responseContent);

        return tokenResponse?.AccessToken ?? throw new Exception("Access token not found in response.");
    }

    private class OAuthTokenResponse
    {
        public string AccessToken { get; set; }
    }
}
