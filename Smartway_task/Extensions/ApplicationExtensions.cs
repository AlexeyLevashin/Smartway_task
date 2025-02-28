using Smartway_task.Services;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        return services;
    }
}