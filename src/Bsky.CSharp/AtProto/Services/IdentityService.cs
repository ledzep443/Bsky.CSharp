using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for working with AT Protocol identities.
/// </summary>
public class IdentityService
{
    private readonly XrpcClient _client;
    private const string ResolveHandleEndpoint = "com.atproto.identity.resolveHandle";
    
    /// <summary>
    /// Creates a new identity service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public IdentityService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Resolves a handle (e.g., @alice.bsky.social) to a DID.
    /// </summary>
    /// <param name="handle">The handle to resolve.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The DID associated with the handle.</returns>
    public async Task<string> ResolveHandleAsync(
        string handle, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["handle"] = handle
        };
        
        var response = await _client.GetAsync<Did>(
            ResolveHandleEndpoint, 
            parameters, 
            cancellationToken);
        
        return response.DidValue;
    }
}
