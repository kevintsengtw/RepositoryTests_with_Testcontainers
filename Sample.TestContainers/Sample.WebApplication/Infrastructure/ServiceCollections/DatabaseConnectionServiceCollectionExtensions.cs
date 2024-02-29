using Sample.WebApplication.Infrastructure.Settings;

namespace Sample.WebApplication.Infrastructure.ServiceCollections;

/// <summary>
/// class DatabaseConnectionServiceCollectionExtensions
/// </summary>
public static class DatabaseConnectionServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Database Connection Settings.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDatabaseConnectionOptions(this IServiceCollection services)
    {
        services.AddOptions<DatabaseConnectionOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    var connectionString = configuration.GetConnectionString("Sample");
                    options.ConnectionString = connectionString;
                });

        return services;
    }
}