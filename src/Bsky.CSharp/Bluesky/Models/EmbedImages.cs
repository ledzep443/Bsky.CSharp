using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents embedded images in a post.
/// Implements the app.bsky.embed.images schema.
/// </summary>
public class EmbedImages : Embed
{
    /// <summary>
    /// The list of embedded images.
    /// </summary>
    [JsonPropertyName("images")]
    [Required]
    [JsonRequired]
    public List<Image> Images { get; set; }
}
