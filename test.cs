using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Fetch OAuth Token
            string oauthToken = await FetchOAuthTokenAsync();

            // Kafka Producer Configuration
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "your-kafka-broker:9092",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.OAuthBearer,
                // Set the OAuth token dynamically
                SaslOauthbearerConfig = $"token={oauthToken}"
            };

            // Create the Kafka Producer using ProducerBuilder
            using var producer = new ProducerBuilder<string, string>(producerConfig).Build();

            Console.WriteLine("Enter messages to send to Kafka (type 'exit' to quit):");

            while (true)
            {
                string input = Console.ReadLine();
                if (input?.ToLower() == "exit")
                    break;

                try
                {
                    // Send message to Kafka
                    var result = await producer.ProduceAsync("your-topic-name", new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(), // Optional: Use GUID as key
                        Value = input
                    });

                    Console.WriteLine($"Message sent to {result.TopicPartitionOffset}");
                }
                catch (ProduceException<string, string> ex)
                {
                    Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Fetches an OAuth token from an external OAuth provider.
    /// </summary>
    private static async Task<string> FetchOAuthTokenAsync()
    {
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

        HttpResponseMessage response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch OAuth token. HTTP Status: {response.StatusCode}");
        }

        string responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(responseContent);

        if (string.IsNullOrEmpty(tokenResponse?.AccessToken))
        {
            throw new Exception("Access token not found in OAuth response.");
        }

        return tokenResponse.AccessToken;
    }

    /// <summary>
    /// Class to deserialize the OAuth token response.
    /// </summary>
    private class OAuthTokenResponse
    {
        public string AccessToken { get; set; }
    }
}
