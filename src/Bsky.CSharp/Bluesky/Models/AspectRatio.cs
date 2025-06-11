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
    public required float Width { get; init; }
    
    /// <summary>
    /// The height component of the aspect ratio.
    /// </summary>
    [JsonPropertyName("height")]
    public required float Height { get; init; }
}
