using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents viewer-specific information about an actor.
/// </summary>
public record ActorViewer
{
    /// <summary>
    /// Indicates if the viewer is muting the actor.
    /// </summary>
    [JsonPropertyName("muted")]
    public bool? Muted { get; init; }
    
    /// <summary>
    /// Indicates if the viewer is blocking the actor.
    /// </summary>
    [JsonPropertyName("blockedBy")]
    public bool? BlockedBy { get; init; }
    
    /// <summary>
    /// Indicates if the actor is blocking the viewer.
    /// </summary>
    [JsonPropertyName("blocking")]
    public string? Blocking { get; init; }
    
    /// <summary>
    /// Indicates if the viewer is following the actor.
    /// </summary>
    [JsonPropertyName("following")]
    public string? Following { get; init; }
    
    /// <summary>
    /// Indicates if the actor is following the viewer.
    /// </summary>
    [JsonPropertyName("followedBy")]
    public string? FollowedBy { get; init; }
}
