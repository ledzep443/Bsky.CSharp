using Bsky.CSharp.Bluesky.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.Bluesky.Services;



/// <inheritdoc />
public class FeedService : IFeedService
{
    private readonly XrpcClient _client;
    
    
    public FeedService(XrpcClient client)
    {
        _client = client;
    }


    /// <inheritdoc />
    public async Task<Feed> GetTimelineAsync(int? limit = null, string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        // This method should be an alias for GetHomeTimelineAsync
        return await GetHomeTimelineAsync(limit, cursor, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
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
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <inheritdoc />
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
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Feed> SearchPostsAsync(string query, int? limit = null, string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.searchPosts";
        var parameters = new Dictionary<string, string>
        {
            ["q"] = query
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
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
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <inheritdoc />
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
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <inheritdoc />
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
        
        return await _client.GetAsync<Feed>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
}
