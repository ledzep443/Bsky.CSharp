using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Interface for AT Protocol synchronization operations.
/// </summary>
public interface ISyncService
{
    /// <summary>
    /// Gets the head of a repository.
    /// </summary>
    /// <param name="did">The DID of the repository.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The repository head.</returns>
    Task<string> GetRepoHeadAsync(string did, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the blob for a commit.
    /// </summary>
    /// <param name="did">The DID of the repository.</param>
    /// <param name="cid">The CID of the commit.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The commit data.</returns>
    Task<byte[]> GetCommitAsync(string did, string cid, CancellationToken cancellationToken = default);
}
