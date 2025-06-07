using System.Net;
using System.Text;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class ServerServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly ServerService _service;

    public ServerServiceTests()
    {
        // Create a handler that returns server info for the describeServer endpoint
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            if (request.RequestUri.ToString().EndsWith("com.atproto.server.describeServer"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "available": true,
                            "version": "1.0.0",
                            "contact": {
                                "email": "support@example.com"
                            },
                            "links": {
                                "termsOfService": "https://example.com/terms",
                                "privacyPolicy": "https://example.com/privacy"
                            }
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            
            // Default response
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });
        
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://api.bsky.app/xrpc/")
        };
        
        _client = new XrpcClient(httpClient);
        _service = new ServerService(_client);
    }

    [Fact]
    public async Task DescribeServerAsync_CallsClientCorrectly()
    {
        // Act
        var result = await _service.DescribeServerAsync();

        // Assert
        // Verify result
        Assert.True(result.Available);
        Assert.Equal("1.0.0", result.Version);
        Assert.Equal("support@example.com", result.Contact?.Email);
        Assert.Equal("https://example.com/terms", result.Links?.TermsOfService);
        Assert.Equal("https://example.com/privacy", result.Links?.PrivacyPolicy);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.EndsWith("com.atproto.server.describeServer", request.RequestUri.ToString());
    }
}
