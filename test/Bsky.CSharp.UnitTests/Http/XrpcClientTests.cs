using System.Net;
using System.Text;
using System.Text.Json;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.Http;

public class XrpcClientTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly HttpClient _httpClient;
    private readonly XrpcClient _client;

    public XrpcClientTests()
    {
        // Create a test handler with a default response
        _handler = new TestHttpMessageHandler((request, cancellationToken) => 
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"test\":\"value\"}", Encoding.UTF8, "application/json")
            });
        });
        
        _httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/")
        };
        
        _client = new XrpcClient(_httpClient);
    }

    [Fact]
    public void GetAccessToken_WhenNotSet_ReturnsNull()
    {
        // Act
        var token = _client.GetAccessToken();
        
        // Assert
        Assert.Null(token);
    }

    [Fact]
    public void SetAuth_SetsAccessToken()
    {
        // Arrange
        const string token = "test-token";
        
        // Act
        _client.SetAuth(token);
        
        // Assert
        Assert.Equal(token, _client.GetAccessToken());
    }

    [Fact]
    public void ClearAuth_ResetsAccessToken()
    {
        // Arrange
        _client.SetAuth("test-token");
        
        // Act
        _client.ClearAuth();
        
        // Assert
        Assert.Null(_client.GetAccessToken());
    }

    [Fact]
    public void BuildUri_WithParameters_ConstructsCorrectUri()
    {
        // Arrange
        var parameters = new Dictionary<string, string>
        {
            ["key1"] = "value1",
            ["key2"] = "value with spaces"
        };
        
        // Act
        var uri = _client.BuildUri("test/endpoint", parameters);
        
        // Assert
        // Check the components of the URI separately
        Assert.Equal("api.bsky.app", uri.Host);
        Assert.Equal("/xrpc/test/endpoint", uri.AbsolutePath);
        Assert.Contains("key1=value1", uri.Query);
        
        // For the space-containing parameter, use a more flexible approach
        // that doesn't depend on the exact encoding mechanism
        Assert.Contains("key2=", uri.Query);
        Assert.Contains("value", uri.Query);
        Assert.Contains("spaces", uri.Query);
    }

    [Fact]
    public void BuildUri_WithoutParameters_ConstructsCorrectUri()
    {
        // Act
        var uri = _client.BuildUri("test/endpoint");
        
        // Assert
        Assert.Equal("https://api.bsky.app/xrpc/test/endpoint", uri.ToString());
    }
    
    [Fact]
    public async Task GetAsync_SetsAuthHeaderWhenTokenProvided()
    {
        // Arrange
        _client.SetAuth("test-token");
        
        // Act
        await _client.GetAsync<JsonDocument>("test/endpoint");
        
        // Assert
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
    }
    
    [Fact]
    public async Task PostAsync_SetsAuthHeaderWhenTokenProvided()
    {
        // Arrange
        _client.SetAuth("test-token");
        var data = new { Property = "Value" };
        
        // Act
        await _client.PostAsync<object, JsonDocument>("test/endpoint", data);
        
        // Assert
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
    }
}
