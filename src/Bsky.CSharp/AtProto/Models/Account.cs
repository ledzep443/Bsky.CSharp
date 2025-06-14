using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents an account in the AT Protocol.
/// </summary>
public class Account
{
    /// <summary>
    /// The Decentralized Identifier (DID) of the account.
    /// </summary>
    [JsonPropertyName("did")]
    [Required]
    public string? Did { get; set; }
    
    /// <summary>
    /// The human-readable handle of the account.
    /// </summary>
    [JsonPropertyName("handle")]
    [Required]
    public string? Handle { get; set; }
    
    /// <summary>
    /// The email associated with the account (optional).
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    /// <summary>
    /// Indicates if the email has been confirmed.
    /// </summary>
    [JsonPropertyName("emailConfirmed")]
    public bool? EmailConfirmed { get; set; }
}
