using System.Net;
using System.Text;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class SyncServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly SyncService _service;

    public SyncServiceTests()
    {
        // Create handler that returns appropriate responses for different endpoints
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            if (request.RequestUri.ToString().Contains("com.atproto.sync.getBlob"))
            {
                var binaryContent = new ByteArrayContent(new byte[] { 1, 2, 3, 4, 5 });
                binaryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = binaryContent
                });
            }
            else if (request.RequestUri.ToString().Contains("com.atproto.sync.getBlocks"))
            {
                var binaryContent = new ByteArrayContent(new byte[] { 10, 20, 30, 40, 50 });
                binaryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = binaryContent
                });
            }
            else if (request.RequestUri.ToString().Contains("com.atproto.sync.getRepo"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "root": "cid-root",
                            "head": "cid-head"
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().Contains("com.atproto.sync.requestCrawl"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            else if (request.RequestUri.ToString().Contains("com.atproto.sync.notifyOfUpdate"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
            
            // Default response for unexpected requests
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });
        
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/xrpc/")
        };
        
        _client = new XrpcClient(httpClient);
        _service = new SyncService(_client);
    }

    [Fact]
    public async Task GetBlobAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string did = "did:plc:test";
        const string cid = "bafytest";
        _client.SetAuth("test-token");
        
        // Act
        var result = await _service.GetBlobAsync(did, cid);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Length);
        Assert.Equal(1, result[0]);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.sync.getBlob", request.RequestUri.ToString());
        Assert.Contains($"did={Uri.EscapeDataString(did)}", request.RequestUri.Query);
        Assert.Contains($"cid={Uri.EscapeDataString(cid)}", request.RequestUri.Query);
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task GetCommitAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string did = "did:plc:test";
        const string cid = "bafytest";
        _client.SetAuth("test-token");
        
        // Act
        var result = await _service.GetCommitAsync(did, cid);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Length);
        Assert.Equal(10, result[0]);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.sync.getBlocks", request.RequestUri.ToString());
        Assert.Contains($"did={Uri.EscapeDataString(did)}", request.RequestUri.Query);
        Assert.Contains($"cid={Uri.EscapeDataString(cid)}", request.RequestUri.Query);
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task GetRepoHeadAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string did = "did:plc:test";
        _client.SetAuth("test-token");
        
        // Act
        var result = await _service.GetRepoHeadAsync(did);
        
        // Assert
        Assert.Equal("cid-head", result);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.sync.getRepo", request.RequestUri.ToString());
        Assert.Contains($"did={Uri.EscapeDataString(did)}", request.RequestUri.Query);
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task RequestCrawlAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string hostname = "test.bsky.network";
        _client.SetAuth("test-token");
        
        // Act
        await _service.RequestCrawlAsync(hostname);
        
        // Assert
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Contains("com.atproto.sync.requestCrawl", request.RequestUri.ToString());
        
        // Verify request body contains hostname
        var content = await request.Content.ReadAsStringAsync();
        Assert.Contains(hostname, content);
    }

    [Fact]
    public async Task NotifyOfUpdateAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string hostname = "test.bsky.network";
        const string did = "did:plc:test";
        _client.SetAuth("test-token");
        
        // Act
        await _service.NotifyOfUpdateAsync(hostname, did);
        
        // Assert
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Contains("com.atproto.sync.notifyOfUpdate", request.RequestUri.ToString());
        
        // Verify request body contains hostname and did
        var content = await request.Content.ReadAsStringAsync();
        Assert.Contains(hostname, content);
        Assert.Contains(did, content);
    }
}
