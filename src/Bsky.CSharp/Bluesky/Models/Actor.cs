using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents an actor (user) in the Bluesky network.
/// </summary>
public record Actor
{
    /// <summary>
    /// The DID of the actor.
    /// </summary>
    [JsonPropertyName("did")]
    public required string Did { get; init; }
    
    /// <summary>
    /// The handle of the actor.
    /// </summary>
    [JsonPropertyName("handle")]
    public required string Handle { get; init; }
    
    /// <summary>
    /// The display name of the actor.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }
    
    /// <summary>
    /// The description/bio of the actor.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    /// <summary>
    /// The avatar image of the actor.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; init; }
    
    /// <summary>
    /// The banner image of the actor.
    /// </summary>
    [JsonPropertyName("banner")]
    public string? Banner { get; init; }
    
    /// <summary>
    /// The number of followers the actor has.
    /// </summary>
    [JsonPropertyName("followersCount")]
    public int? FollowersCount { get; init; }
    
    /// <summary>
    /// The number of accounts the actor follows.
    /// </summary>
    [JsonPropertyName("followsCount")]
    public int? FollowsCount { get; init; }
    
    /// <summary>
    /// The number of posts the actor has created.
    /// </summary>
    [JsonPropertyName("postsCount")]
    public int? PostsCount { get; init; }
    
    /// <summary>
    /// The timestamp when the actor was indexed by the PDS.
    /// </summary>
    [JsonPropertyName("indexedAt")]
    public DateTime? IndexedAt { get; init; }
    
    /// <summary>
    /// Viewer-specific state (if the viewer is authenticated).
    /// </summary>
    [JsonPropertyName("viewer")]
    public ActorViewer? Viewer { get; init; }
}
