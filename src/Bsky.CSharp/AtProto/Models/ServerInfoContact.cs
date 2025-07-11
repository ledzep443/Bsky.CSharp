using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Contact information for a server.
/// </summary>
public class ServerInfoContact
{
    /// <summary>
    /// The email address for contacting the server administrator.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
