using Dapper;
using ShipmentAPI.Extensions;
using ShipmentAPI.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ShipmentAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public SqlConnection Connection { get; }
        public IDbTransaction DbTransaction { get; }

        public Repository(SqlConnection connection, IDbTransaction dbTransaction)
        {
            this.Connection = connection;
            this.DbTransaction = dbTransaction;
        }

        public int Add(TEntity entity)
        {
            var sql = entity.InsertScript();
            var insertParameters = entity.GetInsertParameters();
            var parameters = new DynamicParameters(insertParameters);
            return this.Connection.ExecuteScalar<int>(sql, parameters, transaction: this.DbTransaction);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var sql = typeof(TEntity).GetAllScript();
            return this.Connection.Query<TEntity>(sql, transaction: this.DbTransaction);
        }
    }
}
