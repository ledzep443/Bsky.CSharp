using System;
using System.ComponentModel.DataAnnotations;
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
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string? Uri { get; set; }
    
    /// <summary>
    /// CID of the post.
    /// </summary>
    [JsonPropertyName("cid"), Required, JsonRequired]
    public string? Cid { get; set; }
    
    /// <summary>
    /// Author of the post.
    /// </summary>
    [JsonPropertyName("author"), Required, JsonRequired]
    public Author? Author { get; set; }
    
    /// <summary>
    /// Record data of the post.
    /// </summary>
    [JsonPropertyName("record"), Required, JsonRequired]
    public PostRecord Record { get; set; }
    
    /// <summary>
    /// Embedded content in the post.
    /// </summary>
    [JsonPropertyName("embed")]
    public Embed? Embed { get; set; }
    
    /// <summary>
    /// When the post was indexed.
    /// </summary>
    [JsonPropertyName("indexedAt"), Required, JsonRequired]
    public DateTime IndexedAt { get; set; }
    
    /// <summary>
    /// Likes count of the post.
    /// </summary>
    [JsonPropertyName("likeCount")]
    public int LikeCount { get; set; }
    
    /// <summary>
    /// Reposts count of the post.
    /// </summary>
    [JsonPropertyName("repostCount")]
    public int RepostCount { get; set; }
    
    /// <summary>
    /// Replies count of the post.
    /// </summary>
    [JsonPropertyName("replyCount")]
    public int ReplyCount { get; set; }
    
    /// <summary>
    /// Whether the current user has liked the post.
    /// </summary>
    [JsonPropertyName("viewer")]
    public PostViewer? Viewer { get; set; }
}
