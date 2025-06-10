using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Interface for resolving and managing identities in the AT Protocol.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Resolves a handle to a DID.
    /// </summary>
    /// <param name="handle">The handle to resolve.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The DID corresponding to the handle.</returns>
    Task<Did> ResolveHandleAsync(string handle, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates the handle for the authenticated user.
    /// </summary>
    /// <param name="handle">The new handle.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the async operation.</returns>
    Task UpdateHandleAsync(string handle, CancellationToken cancellationToken = default);
}
