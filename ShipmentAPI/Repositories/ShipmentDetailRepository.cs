using ShipmentAPI.Entities;
using ShipmentAPI.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ShipmentAPI.Repositories
{
    public class ShipmentDetailRepository : Repository<ShipmentDetail>, IShipmentDetailRepository
    {
        public ShipmentDetailRepository(SqlConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {
        }
    }
}
