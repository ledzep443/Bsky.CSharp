using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents an image used in a post.
/// </summary>
public record Image
{
    /// <summary>
    /// The URL of the image blob.
    /// </summary>
    [JsonPropertyName("image")]
    public required string ImageUrl { get; init; }
    
    /// <summary>
    /// Alternative text for the image.
    /// </summary>
    [JsonPropertyName("alt")]
    public required string Alt { get; init; }
    
    /// <summary>
    /// The aspect ratio of the image.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    public AspectRatio? AspectRatio { get; init; }
}

/// <summary>
/// Represents the aspect ratio of an image.
/// </summary>
public record AspectRatio
{
    /// <summary>
    /// The width component of the aspect ratio.
    /// </summary>
    [JsonPropertyName("width")]
    public required int Width { get; init; }
    
    /// <summary>
    /// The height component of the aspect ratio.
    /// </summary>
    [JsonPropertyName("height")]
    public required int Height { get; init; }
}
