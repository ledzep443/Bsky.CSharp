using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for AT Protocol repository synchronization operations.
/// </summary>
public class SyncService : ISyncService
{
    private readonly XrpcClient _client;
    private const string GetBlobEndpoint = "com.atproto.sync.getBlob";
    private const string GetBlocksEndpoint = "com.atproto.sync.getBlocks";
    private const string GetRepoEndpoint = "com.atproto.sync.getRepo";
    private const string ListBlobsEndpoint = "com.atproto.sync.listBlobs";
    private const string ListReposEndpoint = "com.atproto.sync.listRepos";
    private const string NotifyOfUpdateEndpoint = "com.atproto.sync.notifyOfUpdate";
    private const string RequestCrawlEndpoint = "com.atproto.sync.requestCrawl";
    
    /// <summary>
    /// Creates a new sync service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public SyncService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Gets a blob by repository DID and CID.
    /// </summary>
    /// <param name="did">The repository DID.</param>
    /// <param name="cid">The blob CID.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The blob data as a byte array.</returns>
    public async Task<byte[]> GetBlobAsync(
        string did, 
        string cid, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["did"] = did,
            ["cid"] = cid
        };
        
        // NOTE: This uses a different approach than other methods because it returns raw bytes
        var request = new HttpRequestMessage(HttpMethod.Get, _client.BuildUri(GetBlobEndpoint, parameters));
        
        if (_client.GetAccessToken() != null)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _client.GetAccessToken());
        }
        
        var response = await _client.SendRawRequestAsync(request, cancellationToken).ConfigureAwait(false);
        
        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }
    
    /// <summary>
    /// Requests a crawl of a specific repository.
    /// </summary>
    /// <param name="hostname">The hostname of the service to crawl from.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RequestCrawlAsync(
        string hostname,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Hostname = hostname
        };
        
        await _client.PostAsync<object>(
            RequestCrawlEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <summary>
    /// Notifies the PDS of an update to a repository.
    /// </summary>
    /// <param name="hostname">The hostname of the service.</param>
    /// <param name="did">The repository DID.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task NotifyOfUpdateAsync(
        string hostname,
        string did,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Hostname = hostname,
            Did = did
        };
        
        await _client.PostAsync<object>(
            NotifyOfUpdateEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string> GetRepoHeadAsync(string did, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GetCommitAsync(string did, string cid, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
