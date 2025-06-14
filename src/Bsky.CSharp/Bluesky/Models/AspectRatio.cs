using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents the aspect ratio of an image as width/height.
/// </summary>
public class AspectRatio
{
    /// <summary>
    /// The width component of the aspect ratio.
    /// </summary>
    [JsonPropertyName("width")]
    [Required]
    [JsonRequired]
    public float Width { get; set; }
    
    /// <summary>
    /// The height component of the aspect ratio.
    /// </summary>
    [JsonPropertyName("height")]
    [Required]
    [JsonRequired]
    public float Height { get; set; }
}
