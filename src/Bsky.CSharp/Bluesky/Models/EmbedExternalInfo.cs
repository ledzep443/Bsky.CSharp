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
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The title of the external content.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    /// <summary>
    /// The description of the external content.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    /// <summary>
    /// Thumbnail image information for the external content.
    /// </summary>
    [JsonPropertyName("thumb")]
    public EmbedExternalThumb? Thumb { get; init; }
    
}
