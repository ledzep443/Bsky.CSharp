using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Reference to a created record.
/// </summary>
public class RecordRef
{
    /// <summary>
    /// The URI of the record.
    /// </summary>
    [JsonPropertyName("uri")]
    [Required]
    public string? Uri { get; set; }
    
    /// <summary>
    /// The CID (Content Identifier) of the record.
    /// </summary>
    [JsonPropertyName("cid")]
    [Required]
    public string? Cid { get; set; }
}
