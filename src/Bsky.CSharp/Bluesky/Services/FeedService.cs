using Bsky.CSharp.Bluesky.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.Bluesky.Services;

/// <summary>
/// Service for retrieving Bluesky feed timelines.
/// </summary>
public class FeedService
{
    private readonly XrpcClient _client;
    
    /// <summary>
    /// Creates a new feed service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public FeedService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Gets the user's home timeline.
    /// </summary>
    /// <param name="limit">Maximum number of posts to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A feed of posts.</returns>
    public async Task<Feed> GetHomeTimelineAsync(
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.getTimeline";
        var parameters = new Dictionary<string, string>();
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken);
    }
    
    /// <summary>
    /// Gets the author's feed.
    /// </summary>
    /// <param name="author">The handle or DID of the author.</param>
    /// <param name="limit">Maximum number of posts to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A feed of posts.</returns>
    public async Task<Feed> GetAuthorFeedAsync(
        string author,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.getAuthorFeed";
        var parameters = new Dictionary<string, string>
        {
            ["actor"] = author
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken);
    }
    
    /// <summary>
    /// Gets a custom feed by its URI.
    /// </summary>
    /// <param name="feedUri">The URI of the custom feed.</param>
    /// <param name="limit">Maximum number of posts to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A feed of posts.</returns>
    public async Task<Feed> GetCustomFeedAsync(
        string feedUri,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.getFeed";
        var parameters = new Dictionary<string, string>
        {
            ["feed"] = feedUri
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken);
    }
    
    /// <summary>
    /// Gets posts that match a specific hashtag.
    /// </summary>
    /// <param name="hashtag">The hashtag to search for (without the # symbol).</param>
    /// <param name="limit">Maximum number of posts to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A feed of posts.</returns>
    public async Task<Feed> GetPostsByHashtagAsync(
        string hashtag,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.searchPosts";
        var parameters = new Dictionary<string, string>
        {
            ["q"] = $"#{hashtag}"
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken);
    }
    
    /// <summary>
    /// Gets the liked posts for a user.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of posts to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A feed of posts.</returns>
    public async Task<Feed> GetLikedPostsAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.getLikes";
        var parameters = new Dictionary<string, string>
        {
            ["actor"] = user
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken);
    }
}
