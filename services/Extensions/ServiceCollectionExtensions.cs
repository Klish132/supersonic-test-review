using Microsoft.Extensions.DependencyInjection;
using services.abstractions.Interfaces;
using services.Implementations;

namespace services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IFoldersService, FoldersService>();
        services.AddScoped<INotesService, NotesService>();
        services.AddScoped<IServicesManager, ServicesManager>();
        return services;
    }
}