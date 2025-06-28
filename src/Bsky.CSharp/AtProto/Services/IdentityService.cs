using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for working with AT Protocol identities.
/// </summary>
public class IdentityService : IIdentityService
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
            cancellationToken)
            .ConfigureAwait(false);
        
        return response.DidValue;
    }

    public async Task UpdateHandleAsync(string handle, CancellationToken cancellationToken = default)
    {
        const string endpoint = "com.atproto.identity.updateHandle";
        var request = new { handle };
        
        await _client.PostAsync<object>(
            endpoint,
            request,
            cancellationToken)
            .ConfigureAwait(false);
    }

    async Task<Did> IIdentityService.ResolveHandleAsync(string handle, CancellationToken cancellationToken)
    {
        // Call the public method and convert the result to a Did object
        var didString = await ResolveHandleAsync(handle, cancellationToken).ConfigureAwait(false);
        return new Did { DidValue = didString };
    }
}
