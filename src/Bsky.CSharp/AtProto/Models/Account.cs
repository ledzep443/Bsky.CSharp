using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents an account in the AT Protocol.
/// </summary>
public record Account
{
    /// <summary>
    /// The Decentralized Identifier (DID) of the account.
    /// </summary>
    [JsonPropertyName("did")]
    public required string Did { get; init; }
    
    /// <summary>
    /// The human-readable handle of the account.
    /// </summary>
    [JsonPropertyName("handle")]
    public required string Handle { get; init; }
    
    /// <summary>
    /// The email associated with the account (optional).
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }
    
    /// <summary>
    /// Indicates if the email has been confirmed.
    /// </summary>
    [JsonPropertyName("emailConfirmed")]
    public bool? EmailConfirmed { get; init; }
}
