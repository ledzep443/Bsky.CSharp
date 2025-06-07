using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Reference to a created record.
/// </summary>
public record RecordRef
{
    /// <summary>
    /// The URI of the record.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The CID (Content Identifier) of the record.
    /// </summary>
    [JsonPropertyName("cid")]
    public required string Cid { get; init; }
}
