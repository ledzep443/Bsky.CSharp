using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Information about the viewer's relationship with a post.
/// </summary>
public class PostViewer
{
    /// <summary>
    /// Whether the viewer has liked the post.
    /// </summary>
    [JsonPropertyName("like")]
    public string? Like { get; init; }
    
    /// <summary>
    /// Whether the viewer has reposted the post.
    /// </summary>
    [JsonPropertyName("repost")]
    public string? Repost { get; init; }
}
