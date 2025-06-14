using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a thread view for a post that was not found.
/// </summary>
public class NotFoundThreadView : ThreadView
{
    /// <summary>
    /// The URI of the post that was not found.
    /// </summary>
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string Uri { get; set; }
}
