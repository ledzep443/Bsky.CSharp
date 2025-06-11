using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a thread view for a post that is blocked from viewing.
/// </summary>
public class BlockedThreadView : ThreadView
{
    /// <summary>
    /// The URI of the blocked post.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The author's DID.
    /// </summary>
    [JsonPropertyName("author")]
    public required Author Author { get; init; }
}
