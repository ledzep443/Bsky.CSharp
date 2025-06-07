using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a feed of posts.
/// </summary>
public record Feed
{
    /// <summary>
    /// The list of posts in the feed.
    /// </summary>
    [JsonPropertyName("feed")]
    public required List<FeedView> FeedList { get; init; }
    
    /// <summary>
    /// Token for pagination to the next page of results.
    /// </summary>
    [JsonPropertyName("cursor")]
    public string? Cursor { get; init; }
}

/// <summary>
/// Represents a single item in a feed view.
/// </summary>
public record FeedView
{
    /// <summary>
    /// The post in this feed item.
    /// </summary>
    [JsonPropertyName("post")]
    public required Post Post { get; init; }
    
    /// <summary>
    /// The reason this post appears in the feed (e.g., a repost).
    /// </summary>
    [JsonPropertyName("reason")]
    public ReasonRepost? Reason { get; init; }
}

/// <summary>
/// Base class for reasons a post appears in a feed.
/// </summary>
[JsonConverter(typeof(ReasonConverter))]
public abstract record Reason
{
    /// <summary>
    /// The type of reason.
    /// </summary>
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
}

/// <summary>
/// Represents a post appearing in a feed because it was reposted.
/// </summary>
public record ReasonRepost : Reason
{
    /// <summary>
    /// The actor who reposted the content.
    /// </summary>
    [JsonPropertyName("by")]
    public required Actor By { get; init; }
    
    /// <summary>
    /// When the repost occurred.
    /// </summary>
    [JsonPropertyName("indexedAt")]
    public required DateTime IndexedAt { get; init; }
}
