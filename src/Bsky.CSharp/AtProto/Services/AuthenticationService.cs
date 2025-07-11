using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for handling AT Protocol authentication and sessions.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly XrpcClient _client;
    private const string CreateSessionEndpoint = "com.atproto.server.createSession";
    private const string RefreshSessionEndpoint = "com.atproto.server.refreshSession";
    private const string DeleteSessionEndpoint = "com.atproto.server.deleteSession";
    
    /// <summary>
    /// Creates a new authentication service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public AuthenticationService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Creates a new session by authenticating with identifier (handle or email) and password.
    /// </summary>
    /// <param name="identifier">The user identifier (handle or email).</param>
    /// <param name="password">The user password.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The authentication token and account information.</returns>
    public async Task<ServiceAuthToken> CreateSessionAsync(string identifier, string password, CancellationToken cancellationToken = default)
    {
        var request = new SessionCreateRequest
        {
            Identifier = identifier,
            Password = password
        };
        
        var response = await _client.PostAsync<SessionCreateRequest, ServiceAuthToken>(
            CreateSessionEndpoint, 
            request, 
            cancellationToken)
            .ConfigureAwait(false);
        
        // Set both the access token and refresh token in the client for subsequent requests
        _client.SetAuth(response.AccessToken!);
        _client.SetRefreshToken(response.RefreshToken!);
        
        return response;
    }

    public async Task<ServiceAuthToken> RefreshSessionAsync(CancellationToken cancellationToken = default)
    {
        // This implementation uses the current client's refresh token
        // It calls the overloaded method that accepts a refresh token
        var refreshToken = _client.GetRefreshToken();
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new InvalidOperationException("No refresh token available. You must create a session first.");
        }
        
        return await RefreshSessionAsync(refreshToken, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Refreshes the current session using the refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The new authentication token and account information.</returns>
    public async Task<ServiceAuthToken> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // Store the current access token
        var currentToken = _client.GetAccessToken();
        
        try
        {
            // Set the refresh token as the access token for this request
            _client.SetAuth(refreshToken);
            
            var response = await _client.PostAsync<ServiceAuthToken>(
                RefreshSessionEndpoint,
                cancellationToken)
                .ConfigureAwait(false);
            
            // Set the new access token and refresh token in the client
            _client.SetAuth(response.AccessToken!);
            _client.SetRefreshToken(response.RefreshToken!);
            
            return response;
        }
        catch
        {
            // Restore the original token if refresh fails
            if (currentToken != null)
            {
                _client.SetAuth(currentToken);
            }
            
            throw;
        }
    }
    
    /// <summary>
    /// Deletes the current session (logout).
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteSessionAsync(CancellationToken cancellationToken = default)
    {
        await _client.PostAsync<object>(DeleteSessionEndpoint, new {}, cancellationToken).ConfigureAwait(false);
        _client.ClearAuth();
    }
}
