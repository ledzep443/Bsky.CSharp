using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Information about the current user session.
/// </summary>
public class SessionInfo
{
    /// <summary>
    /// The DID of the authenticated user.
    /// </summary>
    [JsonPropertyName("did")]
    public required string Did { get; init; }
    
    /// <summary>
    /// The handle of the authenticated user.
    /// </summary>
    [JsonPropertyName("handle")]
    public required string Handle { get; init; }
    
    /// <summary>
    /// The email of the authenticated user, if available.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }
    
    /// <summary>
    /// Indicates if the user's email is confirmed.
    /// </summary>
    [JsonPropertyName("emailConfirmed")]
    public bool? EmailConfirmed { get; init; }
    
    /// <summary>
    /// The DID of the user's PDS instance.
    /// </summary>
    [JsonPropertyName("didDoc")]
    public object? DidDoc { get; init; }
}
