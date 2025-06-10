using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Interface for repository operations in the AT Protocol.
/// </summary>
public interface IRepositoryService
{
    /// <summary>
    /// Creates a new record in a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The collection ID.</param>
    /// <param name="record">The record data.</param>
    /// <param name="rkey">Optional record key.</param>
    /// <param name="validate">Whether to validate the record.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the created record.</returns>
    Task<RecordRef> CreateRecordAsync(
        string repo,
        string collection,
        object record,
        string? rkey = null,
        bool validate = true,
        CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Gets a record from a repository.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the record to.</typeparam>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The collection ID.</param>
    /// <param name="rkey">The record key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The record data.</returns>
    Task<T> GetRecordAsync<T>(
        string repo,
        string collection,
        string rkey,
        CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Updates an existing record in a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The collection ID.</param>
    /// <param name="rkey">The record key.</param>
    /// <param name="record">The updated record data.</param>
    /// <param name="validate">Whether to validate the record.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the updated record.</returns>
    Task<RecordRef> PutRecordAsync(
        string repo,
        string collection,
        string rkey,
        object record,
        bool validate = true,
        CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Deletes a record from a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The collection ID.</param>
    /// <param name="rkey">The record key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the async operation.</returns>
    Task DeleteRecordAsync(
        string repo,
        string collection,
        string rkey,
        CancellationToken cancellationToken = default);
}
