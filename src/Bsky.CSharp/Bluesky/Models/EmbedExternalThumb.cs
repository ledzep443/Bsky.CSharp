using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a thumbnail image for external content in a post.
/// </summary>
public class EmbedExternalThumb
{
    /// <summary>
    /// The URI of the thumbnail image.
    /// </summary>
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string Uri { get; set; }
    
    /// <summary>
    /// The MIME type of the thumbnail image.
    /// </summary>
    [JsonPropertyName("mimeType"), Required, JsonRequired]
    public string MimeType { get; set; }
}
