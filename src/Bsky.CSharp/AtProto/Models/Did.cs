using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents a Decentralized Identifier (DID) response.
/// </summary>
public record Did
{
    /// <summary>
    /// The Decentralized Identifier value.
    /// </summary>
    [JsonPropertyName("did")]
    public required string DidValue { get; init; }
}
