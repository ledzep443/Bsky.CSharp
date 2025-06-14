using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents viewer-specific information about an actor.
/// </summary>
public class ActorViewer
{
    /// <summary>
    /// Indicates if the viewer is muting the actor.
    /// </summary>
    [JsonPropertyName("muted")]
    public bool Muted { get; set; }
    
    /// <summary>
    /// Indicates if the viewer is blocking the actor.
    /// </summary>
    [JsonPropertyName("blockedBy")]
    public bool BlockedBy { get; set; }
    
    /// <summary>
    /// Indicates if the actor is blocking the viewer.
    /// </summary>
    [JsonPropertyName("blocking")]
    public string? Blocking { get; set; }
    
    /// <summary>
    /// Indicates if the viewer is following the actor.
    /// </summary>
    [JsonPropertyName("following")]
    public string? Following { get; set; }
    
    /// <summary>
    /// Indicates if the actor is following the viewer.
    /// </summary>
    [JsonPropertyName("followedBy")]
    public string? FollowedBy { get; set; }
}
