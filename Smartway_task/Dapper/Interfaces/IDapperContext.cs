

using System.Data;

namespace Smartway_task.Dapper.Interfaces;

public interface IDapperContext
{
    public Task<T?> FirstOrDefault<T>(IQueryObject queryObject);
    public Task<List<T>> ListOrEmpty<T>(IQueryObject queryObject);
    public Task Command(IQueryObject queryObject);
    public Task<T> CommandWithResponse<T>(IQueryObject queryObject);
    public Task<T> ExecuteInTransaction<T>(Func<IDbConnection, IDbTransaction, Task<T>> operation);
    public Task ExecuteInTransaction(Func<IDbConnection, IDbTransaction, Task> operation);
    public Task<List<TResult>> QueryWithJoin<T1, T2, TResult>(
        IQueryObject queryObject,
        Func<T1, T2, TResult> map,
        string splitOn = "Id");

    public Task<List<TResult>> QueryWithJoin<T1, T2, T3, TResult>(
        IQueryObject queryObject,
        Func<T1, T2, T3, TResult> map,
        string splitOn = "Id");
}