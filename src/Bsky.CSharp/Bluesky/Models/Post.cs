using System;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a post on Bluesky.
/// </summary>
public class Post
{
    /// <summary>
    /// URI of the post.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// CID of the post.
    /// </summary>
    [JsonPropertyName("cid")]
    public required string Cid { get; init; }
    
    /// <summary>
    /// Author of the post.
    /// </summary>
    [JsonPropertyName("author")]
    public required Author Author { get; init; }
    
    /// <summary>
    /// Record data of the post.
    /// </summary>
    [JsonPropertyName("record")]
    public required PostRecord Record { get; init; }
    
    /// <summary>
    /// Embedded content in the post.
    /// </summary>
    [JsonPropertyName("embed")]
    public Embed? Embed { get; init; }
    
    /// <summary>
    /// When the post was indexed.
    /// </summary>
    [JsonPropertyName("indexedAt")]
    public required DateTime IndexedAt { get; init; }
    
    /// <summary>
    /// Likes count of the post.
    /// </summary>
    [JsonPropertyName("likeCount")]
    public int LikeCount { get; init; }
    
    /// <summary>
    /// Reposts count of the post.
    /// </summary>
    [JsonPropertyName("repostCount")]
    public int RepostCount { get; init; }
    
    /// <summary>
    /// Replies count of the post.
    /// </summary>
    [JsonPropertyName("replyCount")]
    public int ReplyCount { get; init; }
    
    /// <summary>
    /// Whether the current user has liked the post.
    /// </summary>
    [JsonPropertyName("viewer")]
    public PostViewer? Viewer { get; init; }
}
