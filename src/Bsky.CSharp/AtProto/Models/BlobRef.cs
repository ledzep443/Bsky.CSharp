using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Reference to an uploaded blob.
/// </summary>
public record BlobRef
{
    /// <summary>
    /// The blob reference.
    /// </summary>
    [JsonPropertyName("blob")]
    public required BlobData Blob { get; init; }
}

/// <summary>
/// Blob data with type information.
/// </summary>
public record BlobData
{
    /// <summary>
    /// The MIME type of the blob.
    /// </summary>
    [JsonPropertyName("mimeType")]
    public required string MimeType { get; init; }
    
    /// <summary>
    /// The size of the blob in bytes.
    /// </summary>
    [JsonPropertyName("size")]
    public required int Size { get; init; }
    
    /// <summary>
    /// The reference type.
    /// </summary>
    [JsonPropertyName("$type")]
    public string? Type { get; init; }
    
    /// <summary>
    /// The blob reference.
    /// </summary>
    [JsonPropertyName("ref")]
    public RefData? Ref { get; init; }
}

/// <summary>
/// Reference data for a blob.
/// </summary>
public record RefData
{
    /// <summary>
    /// The link to the blob.
    /// </summary>
    [JsonPropertyName("$link")]
    public required string Link { get; init; }
}
