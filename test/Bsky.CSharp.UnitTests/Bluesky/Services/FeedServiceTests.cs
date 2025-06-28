using System.Net;
using System.Text;
using Bsky.CSharp.Bluesky.Models;
using Bsky.CSharp.Bluesky.Services;
using Bsky.CSharp.Http;
using Bsky.CSharp.UnitTests.TestUtilities;

namespace Bsky.CSharp.UnitTests.Bluesky.Services;

public class FeedServiceTests
{
    private readonly TestHttpMessageHandler _handler;
    private readonly XrpcClient _client;
    private readonly FeedService _service;

    public FeedServiceTests()
    {
        // Create a handler that returns responses based on the endpoint
        _handler = new TestHttpMessageHandler((request, cancellationToken) =>
        {
            // Return different mock responses based on the endpoint called
            if (request.RequestUri.ToString().Contains("app.bsky.feed.getTimeline"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "feed": [
                                {
                                    "post": {
                                        "uri": "at://did:plc:test/app.bsky.feed.post/timeline1",
                                        "cid": "cid1",
                                        "author": {
                                            "did": "did:plc:test",
                                            "handle": "test.bsky.app"
                                        },
                                        "record": {
                                            "text": "Timeline post 1",
                                            "createdAt": "2023-01-01T00:00:00Z"
                                        },
                                        "indexedAt": "2023-01-01T00:00:01Z"
                                    }
                                },
                                {
                                    "post": {
                                        "uri": "at://did:plc:test/app.bsky.feed.post/timeline2",
                                        "cid": "cid2",
                                        "author": {
                                            "did": "did:plc:test",
                                            "handle": "test.bsky.app"
                                        },
                                        "record": {
                                            "text": "Timeline post 2",
                                            "createdAt": "2023-01-01T00:00:00Z"
                                        },
                                        "indexedAt": "2023-01-01T00:00:01Z"
                                    }
                                }
                            ],
                            "cursor": "timeline-cursor"
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().Contains("app.bsky.feed.getAuthorFeed"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "feed": [
                                {
                                    "post": {
                                        "uri": "at://did:plc:test/app.bsky.feed.post/author1",
                                        "cid": "cid1",
                                        "author": {
                                            "did": "did:plc:test",
                                            "handle": "test.bsky.app"
                                        },
                                        "record": {
                                            "text": "Author post 1",
                                            "createdAt": "2023-01-01T00:00:00Z"
                                        },
                                        "indexedAt": "2023-01-01T00:00:01Z"
                                    }
                                }
                            ],
                            "cursor": "author-cursor"
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().Contains("app.bsky.feed.searchPosts"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "feed": [
                                {
                                    "post": {
                                        "uri": "at://did:plc:test/app.bsky.feed.post/search1",
                                        "cid": "cid1",
                                        "author": {
                                            "did": "did:plc:test",
                                            "handle": "test.bsky.app"
                                        },
                                        "record": {
                                            "text": "Search result 1",
                                            "createdAt": "2023-01-01T00:00:00Z"
                                        },
                                        "indexedAt": "2023-01-01T00:00:01Z"
                                    }
                                }
                            ],
                            "cursor": "search-cursor"
                        }
                        """, Encoding.UTF8, "application/json")
                });
            }
            else if (request.RequestUri.ToString().Contains("app.bsky.feed.getFeed"))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        """
                        {
                            "feed": [
                                {
                                    "post": {
                                        "uri": "at://did:plc:test/app.bsky.feed.post/custom1",
                                        "cid": "cid1",
                                        "author": {
                                            "did": "did:plc:test",
                                            "handle": "test.bsky.app"
                                        },
                                        "record": {
                                            "text": "Custom feed post",
                                            "createdAt": "2023-01-01T00:00:00Z"
                                        },
                                        "indexedAt": "2023-01-01T00:00:01Z"
                                    }
                                }
                            ],
                            "cursor": "custom-cursor"
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
        _service = new FeedService(_client);
    }

    [Fact]
    public async Task GetTimelineAsync_CallsGetHomeTimelineAsync()
    {
        // Act
        var result = await _service.GetTimelineAsync(limit: 10, cursor: "test-cursor");

        // Assert
        // Verify result
        Assert.NotNull(result);
        Assert.Equal(2, result.FeedList.Count);
        Assert.Equal("Timeline post 1", result.FeedList[0].Post.Record.Text);
        Assert.Equal("timeline-cursor", result.Cursor);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("app.bsky.feed.getTimeline", request.RequestUri.ToString());
        Assert.Contains("limit=10", request.RequestUri.ToString());
        Assert.Contains("cursor=test-cursor", request.RequestUri.ToString());
    }

    [Fact]
    public async Task GetHomeTimelineAsync_CallsCorrectEndpoint()
    {
        // Act
        var result = await _service.GetHomeTimelineAsync(limit: 10, cursor: "test-cursor");

        // Assert
        // Verify result
        Assert.NotNull(result);
        Assert.Equal(2, result.FeedList.Count);
        Assert.Equal("Timeline post 1", result.FeedList[0].Post.Record.Text);
        Assert.Equal("timeline-cursor", result.Cursor);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("app.bsky.feed.getTimeline", request.RequestUri.ToString());
        Assert.Contains("limit=10", request.RequestUri.ToString());
        Assert.Contains("cursor=test-cursor", request.RequestUri.ToString());
    }

    [Fact]
    public async Task GetAuthorFeedAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string author = "test.bsky.app";
        
        // Act
        var result = await _service.GetAuthorFeedAsync(author, limit: 10, cursor: "test-cursor");

        // Assert
        // Verify result
        Assert.NotNull(result);
        Assert.Single(result.FeedList);
        Assert.Equal("Author post 1", result.FeedList[0].Post.Record.Text);
        Assert.Equal("author-cursor", result.Cursor);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("app.bsky.feed.getAuthorFeed", request.RequestUri.ToString());
        Assert.Contains($"actor={author}", request.RequestUri.ToString());
        Assert.Contains("limit=10", request.RequestUri.ToString());
        Assert.Contains("cursor=test-cursor", request.RequestUri.ToString());
    }

    [Fact]
    public async Task SearchPostsAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string query = "test search";
        
        // Act
        var result = await _service.SearchPostsAsync(query, limit: 10, cursor: "test-cursor");

        // Assert
        // Verify result
        Assert.NotNull(result);
        Assert.Single(result.FeedList);
        Assert.Equal("Search result 1", result.FeedList[0].Post.Record.Text);
        Assert.Equal("search-cursor", result.Cursor);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("app.bsky.feed.searchPosts", request.RequestUri.AbsoluteUri);
        Assert.Contains($"q={Uri.EscapeDataString(query)}", request.RequestUri.AbsoluteUri);
        Assert.Contains("limit=10", request.RequestUri.AbsoluteUri);
        Assert.Contains("cursor=test-cursor", request.RequestUri.AbsoluteUri);
    }

    [Fact]
    public async Task GetCustomFeedAsync_CallsCorrectEndpoint()
    {
        // Arrange
        const string feedUri = "at://did:plc:test/app.bsky.feed.generator/custom";
        
        // Act
        var result = await _service.GetCustomFeedAsync(feedUri, limit: 10, cursor: "test-cursor");

        // Assert
        // Verify result
        Assert.NotNull(result);
        Assert.Single(result.FeedList);
        Assert.Equal("Custom feed post", result.FeedList[0].Post.Record.Text);
        Assert.Equal("custom-cursor", result.Cursor);
        
        // Verify request
        Assert.Single(_handler.ProcessedRequests);
        var request = _handler.ProcessedRequests[0];
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Contains("app.bsky.feed.getFeed", request.RequestUri.ToString());
        Assert.Contains($"feed={Uri.EscapeDataString(feedUri)}", request.RequestUri.ToString());
        Assert.Contains("limit=10", request.RequestUri.ToString());
        Assert.Contains("cursor=test-cursor", request.RequestUri.ToString());
    }
}
