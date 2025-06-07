using System.Net;

namespace Bsky.CSharp.UnitTests.TestUtilities;

/// <summary>
/// Test-specific HttpMessageHandler that allows capturing and mocking HTTP requests.
/// </summary>
public class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
    
    /// <summary>
    /// List of requests that have been processed by this handler.
    /// </summary>
    public List<HttpRequestMessage> ProcessedRequests { get; } = new();

    /// <summary>
    /// Creates a new test HTTP handler with the specified response handler function.
    /// </summary>
    /// <param name="handlerFunc">Function that processes requests and returns responses.</param>
    public TestHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
    {
        _handlerFunc = handlerFunc;
    }

    /// <summary>
    /// Creates a new test HTTP handler that returns a fixed JSON response for all requests.
    /// </summary>
    /// <param name="jsonResponse">The JSON string to return in the response body.</param>
    /// <param name="statusCode">HTTP status code to return (default 200 OK).</param>
    public TestHttpMessageHandler(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        : this((request, cancellationToken) =>
        {
            return Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            });
        })
    {
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Save the request for later assertions
        ProcessedRequests.Add(request);
        
        // Call the handler function
        return _handlerFunc(request, cancellationToken);
    }
}
