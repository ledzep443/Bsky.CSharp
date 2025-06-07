using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a reference to a post being replied to.
/// </summary>
public record ReplyRef
{
    /// <summary>
    /// The URI of the referenced post.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The Content Identifier (CID) of the referenced post.
    /// </summary>
    [JsonPropertyName("cid")]
    public required string Cid { get; init; }
}
