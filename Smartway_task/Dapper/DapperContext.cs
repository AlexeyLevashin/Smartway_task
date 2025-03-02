using System.Data;
using Dapper;
using Npgsql;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.Dapper.Interfaces.Settings;

namespace Smartway_task.Dapper;

public class DapperContext : IDapperContext
{
    private readonly IDapperSettings _dapperSettings;
    private NpgsqlConnection _connection;

    public DapperContext(IDapperSettings dapperSettings)
    {
        _dapperSettings = dapperSettings;
        _connection = new NpgsqlConnection(_dapperSettings.ConnectionString);
    }

    public async Task<IDbTransaction> BeginTransaction()
    {
        await _connection.OpenAsync();
        return await _connection.BeginTransactionAsync();
    }

    public async Task<T?> FirstOrDefault<T>(IQueryObject queryObject)
    {
        return await Execute(query => query.QueryFirstOrDefaultAsync<T>(queryObject.Sql, queryObject.Parameters));
    }

    public async Task<List<T>> ListOrEmpty<T>(IQueryObject queryObject)
    {
        return (await Execute(query => query.QueryAsync<T>(queryObject.Sql, queryObject.Parameters))).ToList();
    }

    public async Task Command(IQueryObject queryObject, IDbTransaction? transaction = null)
    {
        await Execute(query => query.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction));
    }

    public async Task<T> CommandWithResponse<T>(IQueryObject queryObject, IDbTransaction? transaction = null)
    {
        return await Execute(query => query.QueryFirstAsync<T>(queryObject.Sql, queryObject.Parameters, transaction));
    }
    

    public async Task<List<TResult>> QueryWithJoin<T1, T2, T3, TResult>(
        IQueryObject queryObject,
        Func<T1, T2, T3, TResult> map,
        string splitOn = "Id")
    {
        return (await Execute(async query =>
        {
            var result = await query.QueryAsync<T1, T2, T3, TResult>(
                queryObject.Sql,
                (item1, item2, item3) => map(item1, item2, item3),
                queryObject.Parameters,
                splitOn: splitOn);

            return result.ToList();
        }));
    }

    private async Task<T> Execute<T>(Func<IDbConnection, Task<T>> query)
    {
        await using var connection = new NpgsqlConnection(_dapperSettings.ConnectionString);
        var res = await query(connection);
        await connection.CloseAsync();
        return res;
    }
}