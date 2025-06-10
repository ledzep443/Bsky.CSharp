using Bsky.CSharp.Bluesky.Services;
using Bsky.CSharp.Configuration;
using Bsky.CSharp.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bsky.CSharp.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBluesky(this IServiceCollection services)
    {
        services.AddHttpClient(BlueskyConstants.BlueskyClientName)
            .AddTypedClient<XrpcClient>();
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<IPostService, PostService>();
        return services;
    }
}