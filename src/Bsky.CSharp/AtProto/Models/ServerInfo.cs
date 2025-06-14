using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Server information response.
/// </summary>
public class ServerInfo
{
    /// <summary>
    /// Server availability details.
    /// </summary>
    [JsonPropertyName("available")]
    public bool? Available { get; set; }
    
    /// <summary>
    /// The version of the server.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }
    
    /// <summary>
    /// Contact information for the server.
    /// </summary>
    [JsonPropertyName("contact")]
    public ServerInfoContact? Contact { get; set; }
    
    /// <summary>
    /// Important links related to the server.
    /// </summary>
    [JsonPropertyName("links")]
    public ServerInfoLinks? Links { get; set; }
}
