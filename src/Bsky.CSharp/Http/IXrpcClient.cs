using System.Text.Json;

namespace Bsky.CSharp.Http;

/// <summary>
/// Interface for client making XRPC requests to AT Protocol servers.
/// </summary>
public interface IXrpcClient
{
    /// <summary>
    /// Gets the current access token.
    /// </summary>
    /// <returns>The current access token, or null if not set.</returns>
    string? GetAccessToken();
    
    /// <summary>
    /// Sets the authentication token for subsequent requests.
    /// </summary>
    /// <param name="accessToken">The access token to use for authentication.</param>
    void SetAuth(string accessToken);
    
    /// <summary>
    /// Gets the current refresh token.
    /// </summary>
    /// <returns>The current refresh token, or null if not set.</returns>
    string? GetRefreshToken();
    
    /// <summary>
    /// Sets the refresh token for session renewal.
    /// </summary>
    /// <param name="refreshToken">The refresh token to store.</param>
    void SetRefreshToken(string refreshToken);
    
    /// <summary>
    /// Clears both the access token and refresh token.
    /// </summary>
    void ClearAuth();
    
    /// <summary>
    /// Makes a GET request to the specified endpoint.
    /// </summary>
    /// <typeparam name="TResponse">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The endpoint to call.</param>
    /// <param name="parameters">Optional query parameters.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The deserialized response.</returns>
    Task<TResponse> GetAsync<TResponse>(string endpoint, Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes a POST request to the specified endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request body.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The endpoint to call.</param>
    /// <param name="body">The request body.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The deserialized response.</returns>
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body, CancellationToken cancellationToken = default);
    
   
}
