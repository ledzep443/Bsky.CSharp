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
    [JsonPropertyName("image")]
    public required string ImageUrl { get; init; }
    
    /// <summary>
    /// Alternative text for the image.
    /// </summary>
    [JsonPropertyName("alt")]
    public required string Alt { get; init; }
    
    /// <summary>
    /// Optional aspect ratio of the image as width/height.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    public AspectRatio? AspectRatio { get; init; }
}
