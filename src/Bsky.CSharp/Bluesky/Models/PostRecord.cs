using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents the data record of a post on Bluesky.
/// </summary>
public class PostRecord
{
    /// <summary>
    /// The text content of the post.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }
    
    /// <summary>
    /// When the post was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public required DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Embedded content in the post.
    /// </summary>
    [JsonPropertyName("embed")]
    public Embed? Embed { get; init; }
    
    /// <summary>
    /// Reply reference, if this post is a reply.
    /// </summary>
    [JsonPropertyName("reply")]
    public Reply? Reply { get; init; }
    
    /// <summary>
    /// Language tags for the post.
    /// </summary>
    [JsonPropertyName("langs")]
    public List<string>? Langs { get; init; }
}
