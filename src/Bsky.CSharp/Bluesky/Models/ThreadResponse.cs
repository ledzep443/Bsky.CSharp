using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Response from the getPostThread endpoint.
/// </summary>
public class ThreadResponse
{
    /// <summary>
    /// The thread containing the post.
    /// </summary>
    [JsonPropertyName("thread")]
    public required ThreadView Thread { get; init; }
}
