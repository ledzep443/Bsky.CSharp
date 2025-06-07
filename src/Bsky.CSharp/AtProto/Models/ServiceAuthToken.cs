using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents service authentication token information.
/// </summary>
public record ServiceAuthToken
{
    /// <summary>
    /// The authentication token value.
    /// </summary>
    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; init; }
    
    /// <summary>
    /// The refresh token value.
    /// </summary>
    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; init; }
    
    /// <summary>
    /// The type of token.
    /// </summary>
    [JsonPropertyName("tokenType")]
    public required string TokenType { get; init; }
    
    /// <summary>
    /// When the token expires (in seconds).
    /// </summary>
    [JsonPropertyName("expiresIn")]
    public required int ExpiresIn { get; init; }
    
    /// <summary>
    /// The handle of the authenticated user.
    /// </summary>
    [JsonPropertyName("handle")]
    public required string Handle { get; init; }
    
    /// <summary>
    /// The DID of the authenticated user.
    /// </summary>
    [JsonPropertyName("did")]
    public required string Did { get; init; }
}
