using System.Net;
using System.Net.Http.Headers;
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

            if (request.RequestUri.ToString().Contains("com.atproto.sync.getBlob"))
            {
                var binaryContent = new ByteArrayContent(new byte[] { 10, 20, 30, 40, 50 });
                binaryContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = binaryContent
                });
            }
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

            if (request.RequestUri.ToString().Contains("com.atproto.sync.getBlob"))
            {
                var binaryContent = new ByteArrayContent(new byte[] { 10, 20, 30, 40, 50 });
                binaryContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = binaryContent
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

    [Fact]
    public async Task GetBlobAsync_CallsClientWithCorrectParameters()
    {
        // Arrange
        const string did = "did:plc:test";
        const string cid = "bafytest";
        const string expectedContentType = "image/png";
        var expectedData = new byte[] { 10, 20, 30, 40, 50 };

        // Set the authentication token
        _client.SetAuth("test-token");

        // Act
        var result = await _service.GetBlobAsync(did, cid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedContentType, result.ContentType);
        Assert.Equal(expectedData.Length, result.Data.Length);
        for (int i = 0; i < expectedData.Length; i++)
        {
            Assert.Equal(expectedData[i], result.Data[i]);
        }

        // Verify request properties
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("com.atproto.sync.getBlob", request.RequestUri.ToString());
        Assert.Contains($"did={Uri.EscapeDataString(did)}", request.RequestUri.Query);
        Assert.Contains($"cid={Uri.EscapeDataString(cid)}", request.RequestUri.Query);
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("test-token", request.Headers.Authorization?.Parameter);
    }
}
