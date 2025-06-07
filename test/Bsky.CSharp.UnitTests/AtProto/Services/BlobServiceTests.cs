using System.Net;
using System.Text;
using System.Text.Json;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class BlobServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly BlobService _service;

    public BlobServiceTests()
    {
        // Create handler that returns a predetermined response
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            if (request.RequestUri.ToString().EndsWith("com.atproto.blob.upload"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "blob": {
                                "mimeType": "image/jpeg",
                                "size": 5,
                                "$type": "blob",
                                "ref": {
                                    "$link": "bafyrei..."
                                }
                            }
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            
            // Default response for unexpected requests
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });
        
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/xrpc/")
        };
        
        _client = new XrpcClient(httpClient);
        _service = new BlobService(_client);
    }

    [Fact]
    public async Task UploadBlobAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        var blobData = new byte[] { 1, 2, 3, 4, 5 };
        const string contentType = "image/jpeg";
        
        // Set the authentication token
        _client.SetAuth("test-token");
        
        // Act
        var result = await _service.UploadBlobAsync(blobData, contentType);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Blob);
        Assert.Equal("image/jpeg", result.Blob.MimeType);
        Assert.Equal(5, result.Blob.Size);
        Assert.Equal("blob", result.Blob.Type);
        Assert.NotNull(result.Blob.Ref);
        Assert.Equal("bafyrei...", result.Blob.Ref.Link);
        
        // Verify request properties
        Assert.Single(_handler.ProcessedRequests); // Should have exactly one request
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.blob.upload", request.RequestUri.ToString());
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
        Assert.Equal(contentType, request.Content?.Headers.ContentType?.MediaType);
    }
}
