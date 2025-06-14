using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [JsonPropertyName("text"), Required, JsonRequired]
    public string Text { get; set; }
    
    /// <summary>
    /// When the post was created.
    /// </summary>
    [JsonPropertyName("createdAt"), Required, JsonRequired]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Embedded content in the post.
    /// </summary>
    [JsonPropertyName("embed")]
    public Embed? Embed { get; set; }
    
    /// <summary>
    /// Reply reference, if this post is a reply.
    /// </summary>
    [JsonPropertyName("reply")]
    public Reply? Reply { get; set; }
    
    /// <summary>
    /// Language tags for the post.
    /// </summary>
    [JsonPropertyName("langs")]
    public List<string>? Langs { get; set; }
}
