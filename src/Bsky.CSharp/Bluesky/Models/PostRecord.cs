using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents the record content of a post.
/// </summary>
public record PostRecord
{
    /// <summary>
    /// The text content of the post.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }
    
    /// <summary>
    /// The creation timestamp of the post.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public required DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Text facets for mentions, links, tags, etc.
    /// </summary>
    [JsonPropertyName("facets")]
    public List<Facet>? Facets { get; init; }
    
    /// <summary>
    /// Referenced embedded content.
    /// </summary>
    [JsonPropertyName("embed")]
    public Embed? Embed { get; init; }
    
    /// <summary>
    /// Reference to the post this is replying to.
    /// </summary>
    [JsonPropertyName("reply")]
    public Reply? Reply { get; init; }
    
    /// <summary>
    /// Collection of language tags indicating the language(s) of the post.
    /// </summary>
    [JsonPropertyName("langs")]
    public List<string>? Langs { get; init; }
}
