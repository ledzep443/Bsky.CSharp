using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a rich text feature in a post, such as a mention, link, or tag.
/// </summary>
public class Facet
{
    /// <summary>
    /// The index range in the post text where this facet applies.
    /// </summary>
    [JsonPropertyName("index"), Required, JsonRequired]
    public FacetIndex? Index { get; set; }
    
    /// <summary>
    /// The features of this facet (mention, link, etc.)
    /// </summary>
    [JsonPropertyName("features"), Required, JsonRequired]
    public List<FacetFeature>? Features { get; set; }
}

/// <summary>
/// Represents the starting and ending indices for a facet in text.
/// </summary>
public class FacetIndex
{
    /// <summary>
    /// Starting character position (inclusive).
    /// </summary>
    [JsonPropertyName("byteStart"), Required, JsonRequired]
    public int ByteStart { get; set; }
    
    /// <summary>
    /// Ending character position (exclusive).
    /// </summary>
    [JsonPropertyName("byteEnd"), Required, JsonRequired]
    public int ByteEnd { get; set; }
}
