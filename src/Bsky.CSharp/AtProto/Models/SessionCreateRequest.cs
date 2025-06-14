using System.ComponentModel.DataAnnotations;
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
    [Required]
    public string Identifier { get; set; }
    
    /// <summary>
    /// The password for authentication.
    /// </summary>
    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }
}
