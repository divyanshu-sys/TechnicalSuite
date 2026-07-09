using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace Ts.ShopIn.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(object id);

        void Create(TEntity entity);

        void CreateRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        EntityEntry<TEntity> GetEntityEntry(TEntity entity);

        EntityState GetEntityState(TEntity entity);

        Task<int> GetTotalCountAsync();

        Task<TEntity> FromSqlRawAsync(string operationName, List<SqlParameter> sqlParameters = null);

        Task<List<TEntity>> ListFromSqlRawAsync(string operationName, List<SqlParameter> sqlParameters = null);

        Task<int> ExecuteSqlRawAsync(string operationName, List<SqlParameter> sqlParameters = null);
    }
}
