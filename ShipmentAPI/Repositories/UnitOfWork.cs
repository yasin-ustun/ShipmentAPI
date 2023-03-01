using ShipmentAPI.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace ShipmentAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SqlConnection _Connection;
        private IDbTransaction _dbTransaction;
        private PackageRepository _PackageRepository;
        private ShipmentRepository _ShipmentRepository;
        private ShipmentDetailRepository _ShipmentDetailRepository;
        public UnitOfWork(SqlConnection connection)
        {
            this._Connection = connection;
            this._Connection.Open();
            this._dbTransaction = this._Connection.BeginTransaction();
        }

        public IPackageRepository PackageRepository => _PackageRepository ?? new PackageRepository(this._Connection, this._dbTransaction);
        public IShipmentRepository ShipmentRepository => _ShipmentRepository ?? new ShipmentRepository(this._Connection, this._dbTransaction);
        public IShipmentDetailRepository ShipmentDetailRepository => _ShipmentDetailRepository ?? new ShipmentDetailRepository(this._Connection, this._dbTransaction);

        public void Commit()
        {
            try
            {
                this._dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                this._dbTransaction.Rollback();
                throw ex;
            }
            finally
            {
                this._dbTransaction?.Dispose();
                this._dbTransaction = this._Connection.BeginTransaction();
            }
        }

        public void Dispose()
        {
            this.Commit();
        }
    }
}
