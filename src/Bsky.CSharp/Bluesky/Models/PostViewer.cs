using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents viewer-specific information about a post.
/// </summary>
public record PostViewer
{
    /// <summary>
    /// Indicates if the viewer has liked the post.
    /// </summary>
    [JsonPropertyName("like")]
    public string? Like { get; init; }
    
    /// <summary>
    /// Indicates if the viewer has reposted the post.
    /// </summary>
    [JsonPropertyName("repost")]
    public string? Repost { get; init; }
    
    /// <summary>
    /// Timestamp when the viewer saw this post.
    /// </summary>
    [JsonPropertyName("seen")]
    public bool? Seen { get; init; }
}
