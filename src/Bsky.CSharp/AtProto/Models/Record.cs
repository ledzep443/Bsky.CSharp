using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents a record in the AT Protocol.
/// </summary>
public record Record
{
    /// <summary>
    /// The URI of the record.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The Content Identifier (CID) of the record.
    /// </summary>
    [JsonPropertyName("cid")]
    public required string Cid { get; init; }
    
    /// <summary>
    /// The data content of the record.
    /// </summary>
    [JsonPropertyName("value")]
    public required Dictionary<string, object> Value { get; init; }
}
