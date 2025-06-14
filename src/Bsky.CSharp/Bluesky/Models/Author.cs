using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents an author of content on Bluesky.
/// </summary>
public class Author
{
    /// <summary>
    /// The DID of the author.
    /// </summary>
    [JsonPropertyName("did")]
    [Required]
    [JsonRequired]
    public string Did { get; set; }
    
    /// <summary>
    /// The handle of the author.
    /// </summary>
    [JsonPropertyName("handle")]
    [Required]
    [JsonRequired]
    public string Handle { get; set; }
    
    /// <summary>
    /// The display name of the author.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// The avatar image URL of the author.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    
    /// <summary>
    /// Information about the author's relationship to the viewer.
    /// </summary>
    [JsonPropertyName("viewer")]
    public AuthorViewer? Viewer { get; set; }
}
