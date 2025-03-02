namespace Smartway_task.Dapper.Interfaces;

public interface IQueryObject
{
    public string Sql { get; }
    public object? Parameters { get; }
}