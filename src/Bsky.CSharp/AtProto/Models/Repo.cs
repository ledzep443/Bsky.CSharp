using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents a repository in the AT Protocol.
/// </summary>
public class Repo
{
    /// <summary>
    /// The DID of the repository.
    /// </summary>
    [JsonPropertyName("did")]
    [Required]
    public string? Did { get; set; }
    
    /// <summary>
    /// The handle associated with the repository.
    /// </summary>
    [JsonPropertyName("handle")]
    public string? Handle { get; set; }
    
    /// <summary>
    /// The root CID of the repository.
    /// </summary>
    [JsonPropertyName("rootCid")]
    public string? RootCid { get; set; }
    
    /// <summary>
    /// The description of the repository.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
