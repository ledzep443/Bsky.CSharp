using System.Net;
using System.Text;
using System.Text.Json;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class AuthenticationServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly AuthenticationService _service;

    public AuthenticationServiceTests()
    {
        // Create a handler that returns responses based on the endpoint
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            if (request.RequestUri.ToString().EndsWith("com.atproto.server.createSession"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "accessToken": "test-access-token",
                            "refreshToken": "test-refresh-token",
                            "handle": "test.user.bsky.app",
                            "did": "did:plc:test",
                            "tokenType": "Bearer",
                            "expiresIn": 3600
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().EndsWith("com.atproto.server.refreshSession"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "accessToken": "new-access-token",
                            "refreshToken": "new-refresh-token",
                            "handle": "test.user.bsky.app",
                            "did": "did:plc:test",
                            "tokenType": "Bearer",
                            "expiresIn": 3600
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().EndsWith("com.atproto.server.deleteSession"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            
            // Default response
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });
        
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/xrpc/")
        };
        
        _client = new XrpcClient(httpClient);
        _service = new AuthenticationService(_client);
    }

    [Fact]
    public async Task CreateSessionAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string identifier = "test.user.bsky.app";
        const string password = "password123";

        // Act
        var result = await _service.CreateSessionAsync(identifier, password);

        // Assert
        // Verify result
        Assert.Equal("test-access-token", result.AccessToken);
        Assert.Equal("test-refresh-token", result.RefreshToken);
        Assert.Equal("did:plc:test", result.Did);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.createSession", request.RequestUri.ToString());
        
        // Verify request body contains username and password
        var requestContent = await request.Content.ReadAsStringAsync();
        Assert.Contains(identifier, requestContent);
        Assert.Contains(password, requestContent);
        
        // Verify access token was set in client
        Assert.Equal("test-access-token", _client.GetAccessToken());
        // Verify refresh token was also stored (adding this check)
        Assert.Equal("test-refresh-token", _client.GetRefreshToken());
    }

    [Fact]
    public async Task RefreshSessionAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string refreshToken = "refresh-token";
        _client.SetAuth("current-token"); // Set initial token

        // Act
        var result = await _service.RefreshSessionAsync(refreshToken);

        // Assert
        // Verify result
        Assert.Equal("new-access-token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.refreshSession", request.RequestUri.ToString());
        
        // Verify authorization header contains refresh token
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal(refreshToken, request.Headers.Authorization?.Parameter);
        
        // Verify new access token was set in client
        Assert.Equal("new-access-token", _client.GetAccessToken());
    }

    [Fact]
    public async Task DeleteSessionAsync_CallsClientCorrectly()
    {
        // Arrange
        _client.SetAuth("test-token");

        // Act
        await _service.DeleteSessionAsync();

        // Assert
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.deleteSession", request.RequestUri.ToString());
        
        // Verify auth token was cleared
        Assert.Null(_client.GetAccessToken());
    }

    [Fact]
    public async Task RefreshSessionAsync_WithRefreshToken_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string refreshToken = "test-refresh-token";

        // Act
        var result = await _service.RefreshSessionAsync(refreshToken);

        // Assert
        // Verify result
        Assert.Equal("new-access-token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        Assert.Equal("did:plc:test", result.Did);
        
        // Verify request
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.refreshSession", request.RequestUri.ToString());
        
        // Verify auth header contains refresh token
        Assert.True(request.Headers.Authorization != null);
        Assert.Equal("Bearer", request.Headers.Authorization.Scheme);
        Assert.Equal(refreshToken, request.Headers.Authorization.Parameter);
    }

    [Fact]
    public async Task RefreshSessionAsync_NoParameters_UsesClientRefreshToken()
    {
        // Arrange
        const string refreshToken = "test-refresh-token";
        _client.SetRefreshToken(refreshToken);

        // Act
        var result = await _service.RefreshSessionAsync();

        // Assert
        // Verify result
        Assert.Equal("new-access-token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        
        // Verify request
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.refreshSession", request.RequestUri.ToString());
        
        // Verify auth header contains refresh token
        Assert.True(request.Headers.Authorization != null);
        Assert.Equal("Bearer", request.Headers.Authorization.Scheme);
        Assert.Equal(refreshToken, request.Headers.Authorization.Parameter);
    }

    [Fact]
    public async Task RefreshSessionAsync_NoRefreshToken_ThrowsException()
    {
        // Arrange
        _client.ClearAuth(); // Clear any tokens

        // Act/Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RefreshSessionAsync());
    }
    
    [Fact]
    public async Task DeleteSessionAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string accessToken = "test-access-token";
        _client.SetAuth(accessToken);

        // Act
        await _service.DeleteSessionAsync();

        // Assert
        // Verify request
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.deleteSession", request.RequestUri.ToString());
        
        // Verify auth header contains access token
        Assert.True(request.Headers.Authorization != null);
        Assert.Equal("Bearer", request.Headers.Authorization.Scheme);
        Assert.Equal(accessToken, request.Headers.Authorization.Parameter);
        
        // Verify token was cleared
        Assert.Null(_client.GetAccessToken());
    }
}
