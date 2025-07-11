using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a record embed in a post.
/// Implements the app.bsky.embed.record schema.
/// </summary>
public class EmbedRecord : Embed
{
    /// <summary>
    /// The embedded record reference.
    /// </summary>
    [JsonPropertyName("record")]
    [Required]
    [JsonRequired]
    public RecordRef Record { get; set; }
}
