using System.ComponentModel.DataAnnotations;
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
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string Uri { get; set; }
    
    /// <summary>
    /// The author's DID.
    /// </summary>
    [JsonPropertyName("author"), Required, JsonRequired]
    public Author Author { get; set; }
}
