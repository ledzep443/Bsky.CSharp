using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents a Decentralized Identifier (DID) response.
/// </summary>
public class Did
{
    /// <summary>
    /// The Decentralized Identifier value.
    /// </summary>
    [JsonPropertyName("did")]
    [Required]
    public string? DidValue { get; set; }
}
