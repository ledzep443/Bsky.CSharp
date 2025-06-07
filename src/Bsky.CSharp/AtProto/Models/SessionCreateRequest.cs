using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Request data for AT Protocol server login.
/// </summary>
public class SessionCreateRequest
{
    /// <summary>
    /// The identifier (handle or email) for authentication.
    /// </summary>
    [JsonPropertyName("identifier")]
    public required string Identifier { get; init; }
    
    /// <summary>
    /// The password for authentication.
    /// </summary>
    [JsonPropertyName("password")]
    public required string Password { get; init; }
}
