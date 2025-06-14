using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Reference to an uploaded blob.
/// </summary>
public class BlobRef
{
    /// <summary>
    /// The blob reference.
    /// </summary>
    [JsonPropertyName("blob")]
    [Required]
    public BlobData? Blob { get; set; }
}

/// <summary>
/// Blob data with type information.
/// </summary>
public class BlobData
{
    /// <summary>
    /// The MIME type of the blob.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [Required]
    public string? MimeType { get; set; }
    
    /// <summary>
    /// The size of the blob in bytes.
    /// </summary>
    [JsonPropertyName("size")]
    [Required]
    public int? Size { get; set; }
    
    /// <summary>
    /// The reference type.
    /// </summary>
    [JsonPropertyName("$type")]
    [Required]
    public string? Type { get; set; }
    
    /// <summary>
    /// The blob reference.
    /// </summary>
    [JsonPropertyName("ref")]
    public RefData? Ref { get; set; }
}

/// <summary>
/// Reference data for a blob.
/// </summary>
public class RefData
{
    /// <summary>
    /// The link to the blob.
    /// </summary>
    [JsonPropertyName("$link")]
    [Required]
    public string? Link { get; set; }
}
