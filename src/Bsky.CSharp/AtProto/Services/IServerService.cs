using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Interface for AT Protocol server operations.
/// </summary>
public interface IServerService
{
    /// <summary>
    /// Gets information about the server.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>Information about the server.</returns>
    Task<ServerInfo> GetServerInfoAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates an app-specific password for API access.
    /// </summary>
    /// <param name="name">The name for the app password.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The created app password.</returns>
    Task<AppPassword> CreateAppPasswordAsync(string name, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lists all app-specific passwords for the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of app passwords.</returns>
    Task<List<AppPassword>> ListAppPasswordsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Revokes an app-specific password.
    /// </summary>
    /// <param name="name">The name of the app password to revoke.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the async operation.</returns>
    Task RevokeAppPasswordAsync(string name, CancellationToken cancellationToken = default);
}
