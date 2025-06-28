using System.Net;
using System.Text;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.AtProto.Services;

public class ServerServiceTests
{
    private TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly ServerService _service;

    public ServerServiceTests()
    {
        // Create a handler that returns responses based on the endpoint
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
            else if (request.RequestUri.ToString().EndsWith("com.atproto.server.createAppPassword"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "name": "My App Password",
                            "password": "abc-def-ghi",
                            "createdAt": "2023-01-01T00:00:00Z"
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().EndsWith("com.atproto.server.listAppPasswords"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "passwords": [
                                {
                                    "name": "App 1",
                                    "createdAt": "2023-01-01T00:00:00Z"
                                },
                                {
                                    "name": "App 2",
                                    "createdAt": "2023-02-01T00:00:00Z"
                                }
                            ]
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().EndsWith("com.atproto.server.revokeAppPassword"))
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

    [Fact]
    public async Task GetServerInfoAsync_CallsDescribeServerAsync()
    {
        // Act
        var result = await _service.GetServerInfoAsync();

        // Assert
        // Verify result - should be the same as DescribeServerAsync
        Assert.True(result.Available);
        Assert.Equal("1.0.0", result.Version);
        Assert.Equal("support@example.com", result.Contact?.Email);

        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.EndsWith("com.atproto.server.describeServer", request.RequestUri.ToString());
    }

    [Fact]
    public async Task CreateAppPasswordAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string passwordName = "My App Password";

        // Act
        var result = await _service.CreateAppPasswordAsync(passwordName);

        // Assert
        // Verify result
        Assert.Equal("abc-def-ghi", result.Password);
        Assert.Equal(passwordName, result.Name);
        Assert.NotEqual(default, result.CreatedAt);

        // Verify request
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.createAppPassword", request.RequestUri.ToString());

        // Verify request body contains password name
        var content = await request.Content.ReadAsStringAsync();
        Assert.Contains(passwordName, content);
    }

    [Fact]
    public async Task ListAppPasswordsAsync_CallsCorrectEndpoint()
    {
        
        // Act
        var result = await _service.ListAppPasswordsAsync();

        // Assert
        // Verify result
        Assert.Equal(2, result.Count);
        Assert.Equal("App 1", result[0].Name);
        Assert.Equal("App 2", result[1].Name);
        Assert.Null(result[0].Password); // Password not included in list response

        // Verify request
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.EndsWith("com.atproto.server.listAppPasswords", request.RequestUri.ToString());
    }

    [Fact]
    public async Task RevokeAppPasswordAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string passwordName = "App 1";

        // Act
        await _service.RevokeAppPasswordAsync(passwordName);

        // Assert
        // Verify request
        Assert.True(_handler.ProcessedRequests.Count > 0);
        var request = _handler.ProcessedRequests.Last();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.EndsWith("com.atproto.server.revokeAppPassword", request.RequestUri.ToString());

        // Verify request body contains password name
        var content = await request.Content.ReadAsStringAsync();
        Assert.Contains(passwordName, content);
    }

}
