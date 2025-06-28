using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for working with AT Protocol repositories.
/// </summary>
public class RepositoryService : IRepositoryService
{
    private readonly XrpcClient _client;
    private const string CreateRecordEndpoint = "com.atproto.repo.createRecord";
    private const string DeleteRecordEndpoint = "com.atproto.repo.deleteRecord";
    private const string GetRecordEndpoint = "com.atproto.repo.getRecord";
    private const string ListRecordsEndpoint = "com.atproto.repo.listRecords";
    private const string PutRecordEndpoint = "com.atproto.repo.putRecord";
    
    /// <summary>
    /// Creates a new repository service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public RepositoryService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Creates a new record in a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The NSID of the record collection.</param>
    /// <param name="record">The record to create.</param>
    /// <param name="rkey">Optional record key.</param>
    /// <param name="validate">Whether to validate the record.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>Information about the created record.</returns>
    public async Task<RecordRef> CreateRecordAsync(
        string repo, 
        string collection, 
        object record, 
        string? rkey = null, 
        bool validate = true, 
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Repo = repo,
            Collection = collection,
            Record = record,
            Rkey = rkey,
            Validate = validate
        };
        
        return await _client.PostAsync<object, RecordRef>(
            CreateRecordEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a record from a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The NSID of the record collection.</param>
    /// <param name="rkey">The record key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The record data.</returns>
    public async Task<Record> GetRecordAsync(
        string repo, 
        string collection, 
        string rkey, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["repo"] = repo,
            ["collection"] = collection,
            ["rkey"] = rkey
        };
        
        return await _client.GetAsync<Record>(
            GetRecordEndpoint, 
            parameters, 
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <summary>
    /// Lists records in a repository collection.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The NSID of the record collection.</param>
    /// <param name="limit">Max number of records to return.</param>
    /// <param name="cursor">Pagination cursor.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of records in the collection.</returns>
    public async Task<Records> ListRecordsAsync(
        string repo, 
        string collection, 
        int? limit = null, 
        string? cursor = null, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["repo"] = repo,
            ["collection"] = collection
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (cursor != null)
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<Records>(
            ListRecordsEndpoint, 
            parameters, 
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <summary>
    /// Deletes a record from a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The NSID of the record collection.</param>
    /// <param name="rkey">The record key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteRecordAsync(
        string repo, 
        string collection, 
        string rkey, 
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Repo = repo,
            Collection = collection,
            Rkey = rkey
        };
        
        await _client.PostAsync<object>(
            DeleteRecordEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <summary>
    /// Updates or creates a record in a repository.
    /// </summary>
    /// <param name="repo">The repository DID.</param>
    /// <param name="collection">The NSID of the record collection.</param>
    /// <param name="rkey">The record key.</param>
    /// <param name="record">The record data.</param>
    /// <param name="validate">Whether to validate the record.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PutRecordAsync(
        string repo, 
        string collection, 
        string rkey, 
        object record, 
        bool validate = true, 
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Repo = repo,
            Collection = collection,
            Rkey = rkey,
            Record = record,
            Validate = validate
        };
        
        await _client.PostAsync<object>(
            PutRecordEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<T> GetRecordAsync<T>(string repo, string collection, string rkey, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["repo"] = repo,
            ["collection"] = collection,
            ["rkey"] = rkey
        };
        
        var response = await _client.GetAsync<RecordWrapper<T>>(
            GetRecordEndpoint, 
            parameters, 
            cancellationToken)
            .ConfigureAwait(false);
        
        return response.Value;
    }

    async Task<RecordRef> IRepositoryService.PutRecordAsync(string repo, string collection, string rkey, object record, bool validate,
        CancellationToken cancellationToken)
    {
        var request = new
        {
            Repo = repo,
            Collection = collection,
            Rkey = rkey,
            Record = record,
            Validate = validate
        };
        
        return await _client.PostAsync<object, RecordRef>(
            PutRecordEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <summary>
    /// Wrapper class to help with generic deserialization from the record endpoint
    /// </summary>
    /// <typeparam name="T">The type of record value</typeparam>
    private class RecordWrapper<T>
    {
        public string Uri { get; set; }
        public string Cid { get; set; }
        public T Value { get; set; }
    }
}
