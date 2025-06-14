using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a feed of posts.
/// </summary>
public class Feed
{
    /// <summary>
    /// The list of posts in the feed.
    /// </summary>
    [JsonPropertyName("feed"), Required, JsonRequired]
    public List<FeedView>? FeedList { get; set; }
    
    /// <summary>
    /// Token for pagination to the next page of results.
    /// </summary>
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
}

/// <summary>
/// Represents a single item in a feed view.
/// </summary>
public class FeedView
{
    /// <summary>
    /// The post in this feed item.
    /// </summary>
    [JsonPropertyName("post"), Required, JsonRequired]
    public Post Post { get; set; }
    
    /// <summary>
    /// The reason this post appears in the feed (e.g., a repost).
    /// </summary>
    [JsonPropertyName("reason")]
    public ReasonRepost? Reason { get; set; }
}

/// <summary>
/// Base class for reasons a post appears in a feed.
/// </summary>
[JsonConverter(typeof(ReasonConverter))]
public abstract class Reason
{
    /// <summary>
    /// The type of reason.
    /// </summary>
    [JsonPropertyName("$type"), Required, JsonRequired]
    public string? Type { get; set; }
}

/// <summary>
/// Represents a post appearing in a feed because it was reposted.
/// </summary>
public class ReasonRepost : Reason
{
    /// <summary>
    /// The actor who reposted the content.
    /// </summary>
    [JsonPropertyName("by"), Required, JsonRequired]
    public Actor? By { get; set; }
    
    /// <summary>
    /// When the repost occurred.
    /// </summary>
    [JsonPropertyName("indexedAt"), Required, JsonRequired]
    public DateTime? IndexedAt { get; set; }
}
