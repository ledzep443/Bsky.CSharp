using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Contains information about an external link embedded in a post.
/// </summary>
public class EmbedExternalInfo
{
    /// <summary>
    /// The URI of the external content.
    /// </summary>
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string Uri { get; set; }
    
    /// <summary>
    /// The title of the external content.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    /// <summary>
    /// The description of the external content.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Thumbnail image information for the external content.
    /// </summary>
    [JsonPropertyName("thumb")]
    public EmbedExternalThumb? Thumb { get; set; }
    
}
