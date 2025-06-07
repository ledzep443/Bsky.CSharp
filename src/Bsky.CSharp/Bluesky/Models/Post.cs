using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a post in the Bluesky network.
/// </summary>
public record Post
{
    /// <summary>
    /// The URI of the post.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The Content Identifier (CID) of the post.
    /// </summary>
    [JsonPropertyName("cid")]
    public required string Cid { get; init; }
    
    /// <summary>
    /// The author of the post.
    /// </summary>
    [JsonPropertyName("author")]
    public required Actor Author { get; init; }
    
    /// <summary>
    /// The actual record content of the post.
    /// </summary>
    [JsonPropertyName("record")]
    public required PostRecord Record { get; init; }
    
    /// <summary>
    /// The timestamp when the post was published.
    /// </summary>
    [JsonPropertyName("indexedAt")]
    public required DateTime IndexedAt { get; init; }
    
    /// <summary>
    /// The parent post reference if this is a reply.
    /// </summary>
    [JsonPropertyName("replyParent")]
    public ReplyRef? ReplyParent { get; init; }
    
    /// <summary>
    /// The root post reference if this is part of a thread.
    /// </summary>
    [JsonPropertyName("replyRoot")]
    public ReplyRef? ReplyRoot { get; init; }
    
    /// <summary>
    /// Viewer-specific state (if the viewer is authenticated).
    /// </summary>
    [JsonPropertyName("viewer")]
    public PostViewer? Viewer { get; init; }
    
    /// <summary>
    /// Reference to embedded content like images, links, or quoted posts.
    /// </summary>
    [JsonPropertyName("embed")]
    public EmbedView? Embed { get; init; }
}
