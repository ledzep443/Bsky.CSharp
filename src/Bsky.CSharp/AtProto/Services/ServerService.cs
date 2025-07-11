using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for interacting with AT Protocol server endpoints.
/// </summary>
public class ServerService : IServerService
{
    private readonly XrpcClient _client;
    private const string DescribeServerEndpoint = "com.atproto.server.describeServer";
    
    /// <summary>
    /// Creates a new server service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public ServerService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Gets information about the server.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>Information about the server.</returns>
    public async Task<ServerInfo> DescribeServerAsync(CancellationToken cancellationToken = default)
    {
        return await _client.GetAsync<ServerInfo>(
            DescribeServerEndpoint,
            null,
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ServerInfo> GetServerInfoAsync(CancellationToken cancellationToken = default)
    {
        // This is just an alias for DescribeServerAsync to match naming conventions in other APIs
        return await DescribeServerAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<AppPassword> CreateAppPasswordAsync(string name, CancellationToken cancellationToken = default)
    {
        const string endpoint = "com.atproto.server.createAppPassword";
        var request = new { name };
        
        return await _client.PostAsync<object, AppPassword>(
            endpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<AppPassword>> ListAppPasswordsAsync(CancellationToken cancellationToken = default)
    {
        const string endpoint = "com.atproto.server.listAppPasswords";
        
        var response = await _client.GetAsync<AppPasswordsResponse>(
            endpoint, 
            null, 
            cancellationToken)
            .ConfigureAwait(false);
        
        return response.Passwords;
    }

    public async Task RevokeAppPasswordAsync(string name, CancellationToken cancellationToken = default)
    {
        const string endpoint = "com.atproto.server.revokeAppPassword";
        var request = new { name };
        
        await _client.PostAsync<object>(
            endpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
    }

    private class AppPasswordsResponse
    {
        public List<AppPassword> Passwords { get; set; } = new List<AppPassword>();
    }
}
