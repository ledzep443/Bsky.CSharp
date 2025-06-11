using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Information about the viewer's relationship with an author.
/// </summary>
public class AuthorViewer
{
    /// <summary>
    /// Whether the viewer is following the author.
    /// </summary>
    [JsonPropertyName("following")]
    public string? Following { get; init; }
    
    /// <summary>
    /// Whether the viewer is followed by the author.
    /// </summary>
    [JsonPropertyName("followedBy")]
    public string? FollowedBy { get; init; }
    
    /// <summary>
    /// Whether the viewer has muted the author.
    /// </summary>
    [JsonPropertyName("muted")]
    public bool? Muted { get; init; }
    
    /// <summary>
    /// Whether the viewer has blocked the author.
    /// </summary>
    [JsonPropertyName("blockedBy")]
    public bool? BlockedBy { get; init; }
}
