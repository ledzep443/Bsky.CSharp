using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents a record in the AT Protocol.
/// </summary>
public class Record
{
    /// <summary>
    /// The URI of the record.
    /// </summary>
    [JsonPropertyName("uri")]
    [Required]
    public string? Uri { get; set; }
    
    /// <summary>
    /// The Content Identifier (CID) of the record.
    /// </summary>
    [JsonPropertyName("cid")]
    [Required]
    public string? Cid { get; set; }
    
    /// <summary>
    /// The data content of the record.
    /// </summary>
    [JsonPropertyName("value")]
    [Required]
    public Dictionary<string, object>? Value { get; set; }
}
