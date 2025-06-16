using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Bluesky.Models;

namespace Bsky.CSharp.Bluesky.Services;

/// <summary>
/// Interface for managing user accounts and profiles on Bluesky.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the current user's profile.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user's profile.</returns>
    Task<Actor> GetMyProfileAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a user's profile by handle or DID.
    /// </summary>
    /// <param name="handleOrDid">The handle or DID of the user.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user's profile.</returns>
    Task<Actor> GetProfileAsync(string handleOrDid, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="update">The profile update information.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the updated profile record.</returns>
    Task<ProfileRecord> UpdateProfileAsync(ProfileUpdate update, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Searches for users by handle or display name.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of matching users.</returns>
    Task<ActorsResponse> SearchUsersAsync(
        string query,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a list of users that the specified user follows.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of users that the specified user follows.</returns>
    Task<ActorsResponse> GetFollowsAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a list of users that follow the specified user.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of users that follow the specified user.</returns>
    Task<ActorsResponse> GetFollowersAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Follows a specified user.
    /// </summary>
    /// <param name="user">The identifier (handle or DID) of the user to follow.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the created follow record.</returns>
    Task<RecordRef> FollowUserAsync(string user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unfollows a user.
    /// </summary>
    /// <param name="user">The handle or DID of the user to unfollow.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UnfollowUserAsync(string user, CancellationToken cancellationToken = default);
}
