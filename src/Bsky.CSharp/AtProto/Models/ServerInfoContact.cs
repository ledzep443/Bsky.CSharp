using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Contact information for a server.
/// </summary>
public record ServerInfoContact
{
    /// <summary>
    /// The email address for contacting the server administrator.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }
}
