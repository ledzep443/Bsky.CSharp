using System.ComponentModel.DataAnnotations;
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
    [JsonPropertyName("did"), Required, JsonRequired]
    public string Did { get; set; }
    
    /// <summary>
    /// The handle of the authenticated user.
    /// </summary>
    [JsonPropertyName("handle"), Required, JsonRequired]
    public string Handle { get; set; }
    
    /// <summary>
    /// The email of the authenticated user, if available.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    /// <summary>
    /// Indicates if the user's email is confirmed.
    /// </summary>
    [JsonPropertyName("emailConfirmed")]
    public bool? EmailConfirmed { get; set; }
    
    /// <summary>
    /// The DID of the user's PDS instance.
    /// </summary>
    [JsonPropertyName("didDoc")]
    public object? DidDoc { get; set; }
}
