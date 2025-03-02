using Smartway_task.Dapper.Interfaces.Settings;

namespace Smartway_task.Dapper;

public class DapperSettings : IDapperSettings
{
    public DapperSettings(IConfiguration configuration)
    {
        ConnectionString = configuration["ConnectionStrings:Database"]
                           ?? throw new ArgumentException("CoonectionString in appsettings is missing");
    }

    public string ConnectionString { get; }
}