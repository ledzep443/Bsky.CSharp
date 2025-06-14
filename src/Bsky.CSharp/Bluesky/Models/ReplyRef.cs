using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a reference to a post being replied to.
/// </summary>
public class ReplyRef
{
    /// <summary>
    /// The URI of the referenced post.
    /// </summary>
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string? Uri { get; set; }
    
    /// <summary>
    /// The Content Identifier (CID) of the referenced post.
    /// </summary>
    [JsonPropertyName("cid"), Required, JsonRequired]
    public string? Cid { get; set; }
}
