using Bsky.CSharp.Bluesky.Models;

namespace Bsky.CSharp.Bluesky.Services;

/// <summary>
/// Interface for accessing and managing feeds on Bluesky.
/// </summary>
public interface IFeedService
{
    /// <summary>
    /// Gets the authenticated user's timeline.
    /// </summary>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of timeline posts.</returns>
    Task<Feed> GetTimelineAsync(
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the authenticated user's home timeline (following and suggested content).
    /// </summary>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of timeline posts.</returns>
    Task<Feed> GetHomeTimelineAsync(
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a user's authored posts.
    /// </summary>
    /// <param name="actor">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of the user's posts.</returns>
    Task<Feed> GetAuthorFeedAsync(
        string actor,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a custom user-defined feed based on the specified feed URI.
    /// </summary>
    /// <param name="feedUri">The unique URI identifying the custom feed to retrieve.</param>
    /// <param name="limit">The maximum number of results to return. Optional.</param>
    /// <param name="cursor">The pagination cursor for retrieving the next set of results. Optional.</param>
    /// <param name="cancellationToken">A token to cancel the request. Optional.</param>
    /// <returns>A feed containing the specified posts.</returns>
    Task<Feed> GetCustomFeedAsync(
        string feedUri,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets posts that match a search query.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of matching posts.</returns>
    Task<Feed> SearchPostsAsync(
        string query,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves posts associated with a specific hashtag.
    /// </summary>
    /// <param name="hashtag">The hashtag to search for.</param>
    /// <param name="limit">Maximum number of results to return. Default is no limit.</param>
    /// <param name="cursor">Pagination cursor from a previous request, used for retrieving subsequent results.</param>
    /// <param name="cancellationToken">A token to cancel the request if needed.</param>
    /// <returns>A feed containing posts matching the specified hashtag.</returns>
    Task<Feed> GetPostsByHashtagAsync(
        string hashtag,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the liked posts for a specified user.
    /// </summary>
    /// <param name="user">The username or actor ID to retrieve liked posts for.</param>
    /// <param name="limit">Maximum number of results to return. Optional.</param>
    /// <param name="cursor">Pagination cursor from a previous request. Optional.</param>
    /// <param name="cancellationToken">A token to cancel the request. Optional.</param>
    /// <returns>A feed containing the user's liked posts.</returns>
    Task<Feed> GetLikedPostsAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
}