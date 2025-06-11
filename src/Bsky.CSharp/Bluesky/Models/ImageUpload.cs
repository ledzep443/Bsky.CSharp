using System;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Client-side model for preparing image data to be uploaded to Bluesky.
/// This is not a direct API schema model, but used to prepare data for the blob upload endpoint.
/// </summary>
public class ImageUpload
{
    /// <summary>
    /// The binary data of the image.
    /// </summary>
    public required byte[] Data { get; init; }
    
    /// <summary>
    /// The MIME type of the image.
    /// </summary>
    public required string ContentType { get; init; }
    
    /// <summary>
    /// Alternative text for the image for accessibility.
    /// </summary>
    public string? AltText { get; init; }
    
    /// <summary>
    /// Optional aspect ratio of the image as width/height.
    /// </summary>
    public AspectRatio? AspectRatio { get; init; }
}
