using System.Net;
using System.Text;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class RepositoryServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly RepositoryService _service;

    public RepositoryServiceTests()
    {
        // Create handler that returns predefined responses based on the endpoint
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            const string repo = "did:plc:test";
            const string collection = "app.bsky.feed.post";
            const string rkey = "unique-key";
            
            if (request.RequestUri?.ToString().Contains("com.atproto.repo.createRecord") == true)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        $"{{\"uri\":\"at://{repo}/{collection}/{rkey}\",\"cid\":\"bafyrei...\"}}",
                        Encoding.UTF8, 
                        "application/json")
                });
            }
            else if (request.RequestUri?.ToString().Contains("com.atproto.repo.getRecord") == true)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        $"{{\"uri\":\"at://{repo}/{collection}/{rkey}\",\"cid\":\"bafyrei...\",\"value\":{{\"text\":\"Hello, world!\"}}}}",
                        Encoding.UTF8, 
                        "application/json")
                });
            }
            else if (request.RequestUri?.ToString().Contains("com.atproto.repo.listRecords") == true)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        $"{{\"records\":[{{\"uri\":\"at://{repo}/{collection}/1\",\"cid\":\"bafyrei-1\",\"value\":{{\"text\":\"First post\"}}}}," +
                        $"{{\"uri\":\"at://{repo}/{collection}/2\",\"cid\":\"bafyrei-2\",\"value\":{{\"text\":\"Second post\"}}}}]," +
                        $"\"cursor\":\"next-cursor\"}}",
                        Encoding.UTF8, 
                        "application/json")
                });
            }
            else if (request.RequestUri?.ToString().Contains("com.atproto.repo.deleteRecord") == true ||
                     request.RequestUri?.ToString().Contains("com.atproto.repo.putRecord") == true)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                });
            }
            
            // Default response
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });
        
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/")
        };
        
        _client = new XrpcClient(httpClient);
        _service = new RepositoryService(_client);
    }

    [Fact]
    public async Task CreateRecordAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string repo = "did:plc:test";
        const string collection = "app.bsky.feed.post";
        var record = new { Text = "Hello, world!" };
        const string rkey = "unique-key";

        // Act
        var result = await _service.CreateRecordAsync(repo, collection, record, rkey);

        // Assert
        // Verify result
        Assert.Equal($"at://{repo}/{collection}/{rkey}", result.Uri);
        Assert.Equal("bafyrei...", result.Cid);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Contains("com.atproto.repo.createRecord", request.RequestUri?.ToString() ?? string.Empty);
        
        // Verify request body contains the necessary parameters
        var requestContent = await request.Content?.ReadAsStringAsync() ?? string.Empty;
        Assert.Contains(repo, requestContent);
        Assert.Contains(collection, requestContent);
        Assert.Contains(rkey, requestContent);
        Assert.Contains("Hello, world!", requestContent);
    }

    [Fact]
    public async Task GetRecordAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string repo = "did:plc:test";
        const string collection = "app.bsky.feed.post";
        const string rkey = "unique-key";
        const string expectedText = "Hello, world!"; // Make sure this matches what's in the mock response

        // Act
        var result = await _service.GetRecordAsync(repo, collection, rkey);

        // Assert
        // Verify result
        Assert.Equal($"at://{repo}/{collection}/{rkey}", result.Uri);
        Assert.Equal("bafyrei...", result.Cid);
        Assert.Equal(expectedText, result.Value["text"].ToString());
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.repo.getRecord", request.RequestUri?.ToString() ?? string.Empty);
        
        // Verify query parameters
        var query = request.RequestUri?.Query ?? string.Empty;
        Assert.Contains($"repo={Uri.EscapeDataString(repo)}", query);
        Assert.Contains($"collection={Uri.EscapeDataString(collection)}", query);
        Assert.Contains($"rkey={Uri.EscapeDataString(rkey)}", query);
    }

    [Fact]
    public async Task ListRecordsAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string repo = "did:plc:test";
        const string collection = "app.bsky.feed.post";
        const int limit = 10;
        const string cursor = "next-page";

        // Act
        var result = await _service.ListRecordsAsync(repo, collection, limit, cursor);

        // Assert
        // Verify result
        Assert.Equal(2, result.RecordList.Count);
        Assert.Equal($"at://{repo}/{collection}/1", result.RecordList[0].Uri);
        Assert.Equal($"at://{repo}/{collection}/2", result.RecordList[1].Uri);
        Assert.Equal("next-cursor", result.Cursor);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.repo.listRecords", request.RequestUri?.ToString() ?? string.Empty);
        
        // Verify query parameters
        var query = request.RequestUri?.Query ?? string.Empty;
        Assert.Contains($"repo={Uri.EscapeDataString(repo)}", query);
        Assert.Contains($"collection={Uri.EscapeDataString(collection)}", query);
        Assert.Contains($"limit={limit}", query);
        Assert.Contains($"cursor={Uri.EscapeDataString(cursor)}", query);
    }

    [Fact]
    public async Task DeleteRecordAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string repo = "did:plc:test";
        const string collection = "app.bsky.feed.post";
        const string rkey = "unique-key";

        // Act
        await _service.DeleteRecordAsync(repo, collection, rkey);

        // Assert
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Contains("com.atproto.repo.deleteRecord", request.RequestUri?.ToString() ?? string.Empty);
        
        // Verify request body contains the necessary parameters
        var requestContent = await request.Content?.ReadAsStringAsync() ?? string.Empty;
        Assert.Contains(repo, requestContent);
        Assert.Contains(collection, requestContent);
        Assert.Contains(rkey, requestContent);
    }

    [Fact]
    public async Task PutRecordAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string repo = "did:plc:test";
        const string collection = "app.bsky.feed.post";
        const string rkey = "unique-key";
        var record = new { Text = "Updated post" };

        // Act
        await _service.PutRecordAsync(repo, collection, rkey, record);

        // Assert
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Contains("com.atproto.repo.putRecord", request.RequestUri?.ToString() ?? string.Empty);
        
        // Verify request body contains the necessary parameters
        var requestContent = await request.Content?.ReadAsStringAsync() ?? string.Empty;
        Assert.Contains(repo, requestContent);
        Assert.Contains(collection, requestContent);
        Assert.Contains(rkey, requestContent);
        Assert.Contains("Updated post", requestContent);
    }
}
