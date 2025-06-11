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
    public required string Did { get; init; }
    
    /// <summary>
    /// The handle of the author.
    /// </summary>
    [JsonPropertyName("handle")]
    public required string Handle { get; init; }
    
    /// <summary>
    /// The display name of the author.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }
    
    /// <summary>
    /// The avatar image URL of the author.
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; init; }
    
    /// <summary>
    /// Information about the author's relationship to the viewer.
    /// </summary>
    [JsonPropertyName("viewer")]
    public AuthorViewer? Viewer { get; init; }
}
