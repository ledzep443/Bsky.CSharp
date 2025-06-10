using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for interacting with AT Protocol server endpoints.
/// </summary>
public class ServerService
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
}
