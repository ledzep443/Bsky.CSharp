using System.Text.Json.Serialization;
using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a reply reference in a post.
/// </summary>
public class Reply
{
    /// <summary>
    /// The root (original) post being replied to in a thread.
    /// </summary>
    [JsonPropertyName("root")]
    public required ReplyRef Root { get; init; }
    
    /// <summary>
    /// The immediate parent post being replied to.
    /// </summary>
    [JsonPropertyName("parent")]
    public required ReplyRef Parent { get; init; }
}
