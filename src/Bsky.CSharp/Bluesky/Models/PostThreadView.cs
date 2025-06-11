using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Thread view for a post.
/// </summary>
public class PostThreadView : ThreadView
{
    /// <summary>
    /// The post referenced in the thread.
    /// </summary>
    [JsonPropertyName("post")]
    public required Post Post { get; init; }
    
    /// <summary>
    /// Parent posts in the thread, if any.
    /// </summary>
    [JsonPropertyName("parent")]
    public ThreadView? Parent { get; init; }
    
    /// <summary>
    /// Replies to the post, if any.
    /// </summary>
    [JsonPropertyName("replies")]
    public List<ThreadView>? Replies { get; init; }
}
