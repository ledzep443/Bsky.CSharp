using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Bsky.CSharp.Http;

/// <summary>
/// Client for making XRPC requests to AT Protocol servers.
/// </summary>
public class XrpcClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private string? _accessToken;
    
    /// <summary>
    /// Creates a new XRPC client.
    /// </summary>
    /// <param name="httpClient">The HttpClient to use for requests.</param>
    public XrpcClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }
    
    /// <summary>
    /// Gets the current access token.
    /// </summary>
    /// <returns>The current access token, or null if not set.</returns>
    public string? GetAccessToken()
    {
        return _accessToken;
    }
    
    /// <summary>
    /// Sets the authentication token for subsequent requests.
    /// </summary>
    /// <param name="accessToken">The access token to use for authentication.</param>
    public void SetAuth(string accessToken)
    {
        _accessToken = accessToken;
    }
    
    /// <summary>
    /// Clears the authentication token.
    /// </summary>
    public void ClearAuth()
    {
        _accessToken = null;
    }
    
    /// <summary>
    /// Builds a URI with the specified endpoint and query parameters.
    /// </summary>
    /// <param name="endpoint">The endpoint path.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <returns>The constructed URI.</returns>
    public Uri BuildUri(string endpoint, Dictionary<string, string>? parameters = null)
    {
        // Ensure the path starts with "/xrpc/" prefix
        var path = endpoint;
        if (!path.StartsWith("/"))
            path = "/" + path;
        
        if (!path.StartsWith("/xrpc/", StringComparison.OrdinalIgnoreCase))
            path = "/xrpc" + path;
        
        var uriBuilder = new UriBuilder(_httpClient.BaseAddress!)
        {
            Path = path
        };
        
        // Add query parameters if provided
        if (parameters != null && parameters.Count > 0)
        {
            var query = new StringBuilder();
            foreach (var param in parameters)
            {
                if (query.Length > 0)
                    query.Append('&');
                
                query.Append(Uri.EscapeDataString(param.Key));
                query.Append('=');
                query.Append(Uri.EscapeDataString(param.Value));
            }
            
            uriBuilder.Query = query.ToString();
        }
        
        return uriBuilder.Uri;
    }
    
    /// <summary>
    /// Makes a GET request to the specified XRPC endpoint.
    /// </summary>
    /// <param name="endpoint">The XRPC endpoint.</param>
    /// <param name="parameters">Optional query parameters.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <returns>The deserialized response.</returns>
    public async Task<TResponse> GetAsync<TResponse>(
        string endpoint, 
        Dictionary<string, string>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        // Use the BuildUri method for consistent URI construction
        var uri = BuildUri(endpoint, parameters);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        
        // Add authentication if available
        if (!string.IsNullOrEmpty(_accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<TResponse>(content, _jsonSerializerOptions, cancellationToken);
        
        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response");
        }
        
        return result;
    }
    
    /// <summary>
    /// Makes a POST request to the specified XRPC endpoint.
    /// </summary>
    /// <param name="endpoint">The XRPC endpoint.</param>
    /// <param name="data">The data to send in the request body.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <returns>The deserialized response.</returns>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint, 
        TRequest data,
        CancellationToken cancellationToken = default)
    {
        // Use the BuildUri method for consistent URI construction
        var uri = BuildUri(endpoint);
        
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(data, _jsonSerializerOptions),
                Encoding.UTF8,
                "application/json")
        };
        
        // Add authentication if available
        if (!string.IsNullOrEmpty(_accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<TResponse>(content, _jsonSerializerOptions, cancellationToken);
        
        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response");
        }
        
        return result;
    }
    
    /// <summary>
    /// Makes a POST request to the specified XRPC endpoint with no response body.
    /// </summary>
    /// <param name="endpoint">The XRPC endpoint.</param>
    /// <param name="data">The data to send in the request body.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PostAsync<TRequest>(
        string endpoint, 
        TRequest data,
        CancellationToken cancellationToken = default)
    {
        // Use the BuildUri method for consistent URI construction
        var uri = BuildUri(endpoint);
        
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(data, _jsonSerializerOptions),
                Encoding.UTF8,
                "application/json")
        };
        
        // Add authentication if available
        if (!string.IsNullOrEmpty(_accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    
    /// <summary>
    /// Makes a POST request to the specified XRPC endpoint with no request body.
    /// </summary>
    /// <param name="endpoint">The XRPC endpoint.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <returns>The deserialized response.</returns>
    public async Task<TResponse> PostAsync<TResponse>(
        string endpoint,
        CancellationToken cancellationToken = default)
    {
        // Use the BuildUri method for consistent URI construction
        var uri = BuildUri(endpoint);
        
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        
        // Add authentication if available
        if (!string.IsNullOrEmpty(_accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<TResponse>(content, _jsonSerializerOptions, cancellationToken);
        
        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response");
        }
        
        return result;
    }
    
    /// <summary>
    /// Sends a raw HTTP request and returns the response.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The HTTP response message.</returns>
    public async Task<HttpResponseMessage> SendRawRequestAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response;
    }
}
