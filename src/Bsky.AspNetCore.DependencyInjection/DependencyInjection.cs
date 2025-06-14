using Bsky.AspNetCore.DependencyInjection.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bsky.AspNetCore.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBluesky(this IServiceCollection services, Func<IServiceProvider, BskySettings> settingsFactory)
    {
        services.AddSingleton(settingsFactory);

        services.AddHttpClient(BlueskyConstants.BlueskyClientName)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                BskySettings settings = settingsFactory(serviceProvider);
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(settings.Timeout);
            });

        return services;
    }
}