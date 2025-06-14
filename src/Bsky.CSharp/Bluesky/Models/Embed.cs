using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Base class for embedded content in posts.
/// </summary>
public abstract class Embed
{
    /// <summary>
    /// The type of embedded content.
    /// Following the pattern "app.bsky.embed.*" as specified in the Bluesky API.
    /// </summary>
    [JsonPropertyName("$type"), Required, JsonRequired]
    public string? Type { get; set; }
}
