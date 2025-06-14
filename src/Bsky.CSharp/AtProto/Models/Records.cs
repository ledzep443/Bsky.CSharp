using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Collection of records from a repository.
/// </summary>
public class Records
{
    /// <summary>
    /// The list of records.
    /// </summary>
    [JsonPropertyName("records")]
    [Required]
    public List<RecordWithKey>? RecordList { get; set; }
    
    /// <summary>
    /// Cursor for pagination.
    /// </summary>
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
}

/// <summary>
/// Record with its key and metadata.
/// </summary>
public class RecordWithKey
{
    /// <summary>
    /// The record URI.
    /// </summary>
    [JsonPropertyName("uri")]
    [Required]
    public string? Uri { get; set; }
    
    /// <summary>
    /// The record CID.
    /// </summary>
    [JsonPropertyName("cid")]
    [Required]
    public string? Cid { get; set; }
    
    /// <summary>
    /// The record value.
    /// </summary>
    [JsonPropertyName("value")]
    [Required]
    public Dictionary<string, object>? Value { get; set; }
}
