using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Bluesky.Models;

namespace Bsky.CSharp.Bluesky.Services;

/// <summary>
/// Interface for creating and managing posts on Bluesky.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Creates a new text post.
    /// </summary>
    /// <param name="text">The text content of the post.</param>
    /// <param name="replyTo">Optional reference to a post being replied to.</param>
    /// <param name="languageTags">Optional language tags for the post.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the created post.</returns>
    Task<RecordRef> CreateTextPostAsync(
        string text,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new post with images.
    /// </summary>
    /// <param name="text">The text content of the post.</param>
    /// <param name="images">The images to attach to the post.</param>
    /// <param name="replyTo">Optional reference to a post being replied to.</param>
    /// <param name="languageTags">Optional language tags for the post.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the created post.</returns>
    Task<RecordRef> CreateImagePostAsync(
        string text,
        IEnumerable<ImageUpload> images,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new post with an external link.
    /// </summary>
    /// <param name="text">The text content of the post.</param>
    /// <param name="url">The external URL to link to.</param>
    /// <param name="title">Optional title for the link preview.</param>
    /// <param name="description">Optional description for the link preview.</param>
    /// <param name="thumbnailData">Optional thumbnail image data for the link.</param>
    /// <param name="thumbnailContentType">Content type of the thumbnail image.</param>
    /// <param name="replyTo">Optional reference to a post being replied to.</param>
    /// <param name="languageTags">Optional language tags for the post.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the created post.</returns>
    Task<RecordRef> CreateLinkPostAsync(
        string text,
        string url,
        string? title = null,
        string? description = null,
        byte[]? thumbnailData = null,
        string? thumbnailContentType = null,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a post by its URI.
    /// </summary>
    /// <param name="uri">The URI of the post to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The post data.</returns>
    Task<Post> GetPostAsync(string uri, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a post.
    /// </summary>
    /// <param name="uri">The URI of the post to delete.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeletePostAsync(string uri, CancellationToken cancellationToken = default);
}
