using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents an image in a post.
/// </summary>
public class Image
{
    /// <summary>
    /// The URL of the image.
    /// </summary>
    [JsonPropertyName("image"), Required, JsonRequired]
    public string ImageUrl { get; set; }
    
    /// <summary>
    /// Alternative text for the image.
    /// </summary>
    [JsonPropertyName("alt"), Required, JsonRequired]
    public string Alt { get; set; }
    
    /// <summary>
    /// Optional aspect ratio of the image as width/height.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    public AspectRatio? AspectRatio { get; set; }
}
