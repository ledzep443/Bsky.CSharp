using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Collection of records from a repository.
/// </summary>
public record Records
{
    /// <summary>
    /// The list of records.
    /// </summary>
    [JsonPropertyName("records")]
    public required List<RecordWithKey> RecordList { get; init; }
    
    /// <summary>
    /// Cursor for pagination.
    /// </summary>
    [JsonPropertyName("cursor")]
    public string? Cursor { get; init; }
}

/// <summary>
/// Record with its key and metadata.
/// </summary>
public record RecordWithKey
{
    /// <summary>
    /// The record URI.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The record CID.
    /// </summary>
    [JsonPropertyName("cid")]
    public required string Cid { get; init; }
    
    /// <summary>
    /// The record value.
    /// </summary>
    [JsonPropertyName("value")]
    public required Dictionary<string, object> Value { get; init; }
}
