using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Encodings.Web;
using Ts.ShopIn.Domain.Interfaces;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext dbContext;
        private readonly DbSet<TEntity> _entity;

        protected GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            _entity = dbContext.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _entity.Add(entity);
        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
            _entity.AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
        }

        public Task<int> ExecuteSqlRawAsync(string operationName, List<SqlParameter> sqlParameters = null)
        {
            if (sqlParameters == null)
                sqlParameters = new();

            if (sqlParameters.Count != 0)
                foreach (var sqlParameter in sqlParameters)
                    sqlParameter.Value = HtmlEncoder.Default.Encode(Convert.ToString(sqlParameter.Value));

            sqlParameters.Add(new SqlParameter("operationName", operationName));
            return dbContext.Database.ExecuteSqlRawAsync($"sp" + typeof(TEntity).Name, sqlParameters);
        }

        public Task<TEntity> FromSqlRawAsync(string operationName, List<SqlParameter> sqlParameters = null)
        {
            if (sqlParameters == null)
                sqlParameters = new();

            sqlParameters.Add(new SqlParameter("operationName", operationName));
            return _entity.FromSqlRaw($"sp" + typeof(TEntity).Name, sqlParameters).FirstOrDefaultAsync();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _entity.AsNoTracking().ToListAsync();
        }

        public Task<TEntity> GetAsync(object id)
        {
            return _entity.FindAsync(id).AsTask();
        }

        public EntityEntry<TEntity> GetEntityEntry(TEntity entity)
        {
            return dbContext.Entry(entity);
        }

        public EntityState GetEntityState(TEntity entity)
        {
            return dbContext.Entry(entity).State;
        }

        public Task<int> GetTotalCountAsync()
        {
            return _entity.CountAsync();
        }

        public Task<List<TEntity>> ListFromSqlRawAsync(string operationName, List<SqlParameter> sqlParameters = null)
        {
            if (sqlParameters == null)
                sqlParameters = new();

            sqlParameters.Add(new SqlParameter("operationName", operationName));
            return _entity.FromSqlRaw($"sp" + typeof(TEntity).Name, sqlParameters).ToListAsync();
        }

        public void Update(TEntity entity)
        {
            _entity.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entity.UpdateRange(entities);
        }
    }
}
