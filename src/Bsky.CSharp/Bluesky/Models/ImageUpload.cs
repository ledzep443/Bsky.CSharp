using System;
using System.ComponentModel.DataAnnotations;

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
    [Required]
    public byte[] Data { get; set; }
    
    /// <summary>
    /// The MIME type of the image.
    /// </summary>
    [Required]
    public string ContentType { get; set; }
    
    /// <summary>
    /// Alternative text for the image for accessibility.
    /// </summary>
    public string? AltText { get; set; }
    
    /// <summary>
    /// Optional aspect ratio of the image as width/height.
    /// </summary>
    public AspectRatio? AspectRatio { get; set; }
}
