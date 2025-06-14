using System.ComponentModel.DataAnnotations;
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
    [JsonPropertyName("root"), Required, JsonRequired]
    public ReplyRef Root { get; set; }
    
    /// <summary>
    /// The immediate parent post being replied to.
    /// </summary>
    [JsonPropertyName("parent"), Required, JsonRequired]
    public ReplyRef Parent { get; set; }
}
