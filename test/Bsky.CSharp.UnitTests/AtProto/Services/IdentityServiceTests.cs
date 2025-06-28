using System.Net;
using System.Text;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class IdentityServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly IdentityService _service;

    public IdentityServiceTests()
    {
        // Create a handler that returns a DID response for the resolveHandle endpoint
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            if (request.RequestUri?.ToString().Contains("com.atproto.identity.resolveHandle") == true)
            {
                // Check if the request URL contains the handle parameter
                string? handle = request.RequestUri?.Query
                    ?.TrimStart('?')
                    .Split('&')
                    .Select(param => param.Split('='))
                    .Where(parts => parts.Length == 2 && parts[0] == "handle")
                    .Select(parts => Uri.UnescapeDataString(parts[1]))
                    .FirstOrDefault();
                
                string didValue = handle == "test.user.bsky.app" 
                    ? "did:plc:test" 
                    : $"did:plc:{handle?.Replace('.', '_')}";
                
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        $"{{\"did\":\"{didValue}\"}}",
                        Encoding.UTF8, 
                        "application/json")
                });
            }
            else if (request.RequestUri?.ToString().Contains("com.atproto.identity.updateHandle") == true)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            if (request.RequestUri?.ToString().Contains("com.atproto.identity.resolveHandle") == true)
            {
                // Check if the request URL contains the handle parameter
                string? handle = request.RequestUri?.Query
                    ?.TrimStart('?')
                    .Split('&')
                    .Select(param => param.Split('='))
                    .Where(parts => parts.Length == 2 && parts[0] == "handle")
                    .Select(parts => Uri.UnescapeDataString(parts[1]))
                    .FirstOrDefault();
                
                string didValue = handle == "test.user.bsky.app" 
                    ? "did:plc:test" 
                    : $"did:plc:{handle?.Replace('.', '_')}";
                
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        $"{{\"did\":\"{didValue}\"}}",
                        Encoding.UTF8, 
                        "application/json")
                });
            }
            else if (request.RequestUri?.ToString().Contains("com.atproto.identity.updateHandle") == true)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            
            // Default response
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });
        
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/")
        };
        
        _client = new XrpcClient(httpClient);
        _service = new IdentityService(_client);
    }

    [Fact]
    public async Task ResolveHandleAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string handle = "test.user.bsky.app";
        const string expectedDid = "did:plc:test";

        // Act
        var result = await _service.ResolveHandleAsync(handle);

        // Assert
        // Verify result
        Assert.Equal(expectedDid, result);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.identity.resolveHandle", request.RequestUri?.ToString() ?? "");
        Assert.Contains($"handle={Uri.EscapeDataString(handle)}", request.RequestUri?.Query ?? "");
    }
    
    [Fact]
    public async Task ResolveHandleAsync_WithDifferentHandle_ReturnsCorrectDid()
    {
        // Arrange
        const string handle = "different.user.bsky.app";
        const string expectedDid = "did:plc:different_user_bsky_app";

        // Act
        var result = await _service.ResolveHandleAsync(handle);

        // Assert
        Assert.Equal(expectedDid, result);
    }

    [Fact]
    public async Task IIdentityService_ResolveHandleAsync_ReturnsDidObject()
    {
        // Arrange
        const string handle = "test.user.bsky.app";
        const string expectedDid = "did:plc:test";
        IIdentityService identityService = _service;

        // Act
        var result = await identityService.ResolveHandleAsync(handle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDid, result.DidValue);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.identity.resolveHandle", request.RequestUri?.ToString() ?? "");
        Assert.Contains($"handle={Uri.EscapeDataString(handle)}", request.RequestUri?.Query ?? "");
    }

    [Fact]
    public async Task UpdateHandleAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string newHandle = "new.handle.bsky.app";

        // Act
        await _service.UpdateHandleAsync(newHandle);

        // Assert
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Contains("com.atproto.identity.updateHandle", request.RequestUri?.ToString() ?? "");
        
        // Verify request body contains the new handle
        var content = await request.Content.ReadAsStringAsync();
        Assert.Contains(newHandle, content);
    }
}
