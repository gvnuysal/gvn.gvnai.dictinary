using Gvn.GvnFramework.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Gvn.GvnAI.Dictionary.Application.DependencyInjection;

public static class ApplicationRegistration
{
    public static IServiceCollection AddDictionaryApplication(this IServiceCollection services)
    {
        services.AddApplicationServices(typeof(ApplicationRegistration).Assembly);
        return services;
    }
}
