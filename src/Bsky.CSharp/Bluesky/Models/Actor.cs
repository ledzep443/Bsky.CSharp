using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents an actor (user) in the Bluesky network.
/// </summary>
public class Actor
{
    /// <summary>
    /// The DID of the actor.
    /// </summary>
    [JsonPropertyName("did"), Required]
    public string? Did { get; set; }
    
    /// <summary>
    /// The handle of the actor.
    /// </summary>
    [JsonPropertyName("handle"), Required]
    public string? Handle { get; set; }
    
    /// <summary>
    /// The display name of the actor.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// The description/bio of the actor.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// The avatar image of the actor.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    
    /// <summary>
    /// The banner image of the actor.
    /// </summary>
    [JsonPropertyName("banner")]
    public string? Banner { get; set; }
    
    /// <summary>
    /// The number of followers the actor has.
    /// </summary>
    [JsonPropertyName("followersCount")]
    public int? FollowersCount { get; set; }
    
    /// <summary>
    /// The number of accounts the actor follows.
    /// </summary>
    [JsonPropertyName("followsCount")]
    public int? FollowsCount { get; set; }
    
    /// <summary>
    /// The number of posts the actor has created.
    /// </summary>
    [JsonPropertyName("postsCount")]
    public int? PostsCount { get; set; }
    
    /// <summary>
    /// The timestamp when the actor was indexed by the PDS.
    /// </summary>
    [JsonPropertyName("indexedAt")]
    public DateTime? IndexedAt { get; set; }
    
    /// <summary>
    /// Viewer-specific state (if the viewer is authenticated).
    /// </summary>
    [JsonPropertyName("viewer")]
    public ActorViewer? Viewer { get; set; }
}
