using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents an external link embed in a post.
/// Implements the app.bsky.embed.external schema.
/// </summary>
public class EmbedExternal : Embed
{
    /// <summary>
    /// Information about the external link.
    /// </summary>
    [JsonPropertyName("external"), Required, JsonRequired]
    public EmbedExternalInfo External { get; set; }
}
