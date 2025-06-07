using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents an application password for API authentication.
/// </summary>
public record AppPassword
{
    /// <summary>
    /// The name of the application password.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    /// <summary>
    /// The generated password value.
    /// </summary>
    [JsonPropertyName("password")]
    public string? Password { get; init; }
    
    /// <summary>
    /// The creation date of the application password.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public required DateTime CreatedAt { get; init; }
}
