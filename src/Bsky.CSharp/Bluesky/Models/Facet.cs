using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a rich text feature in a post, such as a mention, link, or tag.
/// </summary>
public record Facet
{
    /// <summary>
    /// The index range in the post text where this facet applies.
    /// </summary>
    [JsonPropertyName("index")]
    public required FacetIndex Index { get; init; }
    
    /// <summary>
    /// The features of this facet (mention, link, etc.)
    /// </summary>
    [JsonPropertyName("features")]
    public required List<FacetFeature> Features { get; init; }
}

/// <summary>
/// Represents the starting and ending indices for a facet in text.
/// </summary>
public record FacetIndex
{
    /// <summary>
    /// Starting character position (inclusive).
    /// </summary>
    [JsonPropertyName("byteStart")]
    public required int ByteStart { get; init; }
    
    /// <summary>
    /// Ending character position (exclusive).
    /// </summary>
    [JsonPropertyName("byteEnd")]
    public required int ByteEnd { get; init; }
}
