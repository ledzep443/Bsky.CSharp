using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents a repository in the AT Protocol.
/// </summary>
public record Repo
{
    /// <summary>
    /// The DID of the repository.
    /// </summary>
    [JsonPropertyName("did")]
    public required string Did { get; init; }
    
    /// <summary>
    /// The handle associated with the repository.
    /// </summary>
    [JsonPropertyName("handle")]
    public required string Handle { get; init; }
    
    /// <summary>
    /// The root CID of the repository.
    /// </summary>
    [JsonPropertyName("rootCid")]
    public string? RootCid { get; init; }
    
    /// <summary>
    /// The description of the repository.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
