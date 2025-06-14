using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bsky.CSharp.Bluesky.Models.Converters;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Base class for thread views.
/// </summary>
[JsonConverter(typeof(ThreadViewConverter))]
public abstract class ThreadView
{
    /// <summary>
    /// The type of thread view.
    /// </summary>
    [JsonPropertyName("$type"), Required, JsonRequired]
    public string Type { get; set; }
}
