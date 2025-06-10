using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Interface for handling AT Protocol authentication and sessions.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Creates a new session by authenticating with identifier (handle or email) and password.
    /// </summary>
    /// <param name="identifier">The user identifier (handle or email).</param>
    /// <param name="password">The user password.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The authentication token and account information.</returns>
    Task<ServiceAuthToken> CreateSessionAsync(string identifier, string password, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Refreshes the current session to extend its validity.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The refreshed authentication token.</returns>
    Task<ServiceAuthToken> RefreshSessionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes the current session, logging the user out.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the async operation.</returns>
    Task DeleteSessionAsync(CancellationToken cancellationToken = default);
}
