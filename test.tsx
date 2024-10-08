using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class HttpClientHelperTests
{
    [Fact]
    public async Task PostAsync_ShouldReturnSuccessResponse()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHandler.Object);
        var requestUri = "https://example.com/api/test";
        var content = new StringContent("{'key':'value'}", System.Text.Encoding.UTF8, "application/json");

        // Setup mock to return a specific response when a POST request is made
        mockHandler
            .Setup(m => m.SendAsync(It.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{'result':'success'}")
            });

        var httpClientHelper = new HttpClientHelper(httpClient); // Your custom helper class that uses HttpClient

        // Act
        var response = await httpClientHelper.PostAsync(requestUri, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("success", responseBody);
    }
}
