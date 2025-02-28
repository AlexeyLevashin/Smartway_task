using System.Reflection;
using FluentMigrator.Runner;
using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.Dapper.Interfaces.Settings;
using Smartway_task.Repositories;
using Smartway_task.Repositories.Interfaces;

namespace Smartway_task.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddDapper(this IServiceCollection services)
    {
        services.AddScoped<IDapperSettings, DapperSettings>();
        services.AddScoped<IDapperContext, DapperContext>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        return services;
    }
    
    public static IServiceCollection AddMigrations(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore().ConfigureRunner(rb =>
                rb.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
        return services;
    }
}