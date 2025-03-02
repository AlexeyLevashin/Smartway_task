using System.Data;

namespace Smartway_task.Dapper.Interfaces;

public interface IDapperContext
{
    public Task<T?> FirstOrDefault<T>(IQueryObject queryObject);
    public Task<List<T>> ListOrEmpty<T>(IQueryObject queryObject);
    public Task Command(IQueryObject queryObject, IDbTransaction? transaction = null);
    public Task<T> CommandWithResponse<T>(IQueryObject queryObject, IDbTransaction? transaction = null);
    public Task<IDbTransaction> BeginTransaction();

    public Task<List<TResult>> QueryWithJoin<T1, T2, T3, TResult>(
        IQueryObject queryObject,
        Func<T1, T2, T3, TResult> map,
        string splitOn = "Id");
}