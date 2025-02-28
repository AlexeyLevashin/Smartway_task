using Smartway_task.Dapper.Interfaces;

namespace Smartway_task.Dapper;

public class QueryObject : IQueryObject
{
    public string Sql { get; }
    public object? Parameters { get; }
    public object? Transaction { get; }
    public QueryObject(string sql, object? parameters = null, object? transaction = null)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentException("sql is missing");
        }
        
        Sql = sql;
        Parameters = parameters;
        Transaction = transaction;
    }
}