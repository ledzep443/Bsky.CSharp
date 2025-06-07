using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a reply to another post.
/// </summary>
public record Reply
{
    /// <summary>
    /// Reference to the root of the thread.
    /// </summary>
    [JsonPropertyName("root")]
    public required ReplyRef Root { get; init; }
    
    /// <summary>
    /// Reference to the immediate parent post being replied to.
    /// </summary>
    [JsonPropertyName("parent")]
    public required ReplyRef Parent { get; init; }
}
