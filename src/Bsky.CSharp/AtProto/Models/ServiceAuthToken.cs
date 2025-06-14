using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents service authentication token information.
/// </summary>
public class ServiceAuthToken
{
    /// <summary>
    /// The authentication token value.
    /// </summary>
    [JsonPropertyName("accessToken")]
    [Required]
    public string? AccessToken { get; set; }
    
    /// <summary>
    /// The refresh token value.
    /// </summary>
    [JsonPropertyName("refreshToken")]
    [Required]
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// The type of token.
    /// </summary>
    [JsonPropertyName("tokenType")]
    [Required]
    public string? TokenType { get; set; }
    
    /// <summary>
    /// When the token expires (in seconds).
    /// </summary>
    [JsonPropertyName("expiresIn")]
    [Required]
    public int? ExpiresIn { get; set; }
    
    /// <summary>
    /// The handle of the authenticated user.
    /// </summary>
    [JsonPropertyName("handle")]
    [Required]
    public string? Handle { get; set; }
    
    /// <summary>
    /// The DID of the authenticated user.
    /// </summary>
    [JsonPropertyName("did")]
    [Required]
    public string? Did { get; set; }
    
    /// <summary>
    /// The expiration date and time of the token.
    /// </summary>
    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; set; }
}
